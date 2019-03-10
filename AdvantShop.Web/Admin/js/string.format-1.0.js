/*******************************************************
 * String.Format 1.0 - JScript аналог C# String.Format
 *
 * Copyright (c) 2010, Dema (Dema.ru)
 * Лицензия LGPL для любого использования
 *
 *******************************************************/
 
// Инициализация форматирования
(function(st)
{
	// Собственно сам метод форматирования, параметров можно задавать много
	// Использовать так: var str = "Строка формата {0}, использование форматирования {1:формат}".format(some1, some2);
	st.format					= function()
	{
		if(arguments.length==0) return this;
		// RegExp для поиска меток
		var placeholder			= /\{(\d+)(?:,([-+]?\d+))?(?:\:([^(^}]+)(?:\(((?:\\\)|[^)])+)\)){0,1}){0,1}\}/g;
		var args				= arguments;
		// Одним проходом ищем, форматируем и вставляем вместо меток
		return this.replace(placeholder, function(m, num, len, f, params){
			m					= args[Number(num)];	// берем значение по номеру
			f					= formatters[f];		// пытаемся определить функцию форматирования
			return fl(f==null?m:f(m, pp((params || '').replace(/\\\)/g, ')').replace(/\\,/g, '\0').split(','), args) ), len);
		});
	};
	// Реализация форматирования "как в C#": var str = String.Format(format, arg0[, arg1[, arg2 ...]]);
	String.Format				= function(format)
	{
		return arguments.length<=1?format:st.format.apply(format, Array.prototype.slice.call(arguments, 1));
	};
	// Добавить формат
	// name    - имя, которое будет использоваться в форматировании
	// func    - функция форматирования, принимает 2 параметра: значение и параметры для форматирования
	// replace - для замены уже зарегистрированного форматирования нужно передать true
	st.format.add				= function(name, func, replace)
	{
		if(formatters[name]!=null && replace!=true) throw 'Format '+name+' exist, use replace=true for replace';
		formatters[name]		= func;
	};
	// Инициализация параметров форматирования
	// param - объект с настройками в виде { формат: настройка }, где "настройка" зависит от формата и может быть как
	// простым значением, так и объектом типа { параметр: значение, ... }. См. описание настроек для конкретного формата.
	String.Format.init			=
	st.format.init				= function(param)
	{
		var f;
		for(var n in param)
		{
			f					= formatters[n];
			if(f!=null && f.init!=null) f.init(param[n]);
		}
	};
	// Получить функцию форматирования по имени
	st.format.get				= function(name)
	{
		return formatters[name];
	};
	///////// PRIVATE /////////
	// RegExp для поиска ссылок на параметры в форматировании
	var paramph					= /^\{(\d+)\}$/;
	// Набор форматирований
	var formatters				= {};
	// Строка для заполнения нужным количеством пробелов
	var sp						= '    ';
	// Поиск меток в параметрах форматирования и замена их на значения
	function pp(params, args)
	{
		var r;
		for(var i=0; i<params.length; i++)
		{
			if( (r = paramph.exec(params[i])) != null )
				params[i]		= args[Number(r[1])];				// Параметр - метка
			else
				params[i]		= params[i].replace(/\0/g, ',');	// Параметр - не метка
		}
		return params;
	}
	// Заполнение пробелами до нужного размера
	function fl(s, len)
	{
		len						= Number(len);
		if(isNaN(len))	return s;
		s						= ''+s;
		var nl					= Math.abs(len)-s.length;
		if(nl<=0)		return s;
		while(sp.length<nl) sp	+= sp;
		return len<0?(s+sp.substring(0, nl)):(sp.substring(0, nl)+s);
	}
	///////// ОПЯТЬ PUBLIC /////////
	// Регистрируем форматирование массивов.
	//   первый параметр - разделитель (необязательный)
	//   второй параметр - имя формата для применения к каждому элементу (необязательный)
	//   третий и далее  - соответственно параметры формата каждого элемента
	st.format.add('arr', function arr(va, params)
	{
		if(va==null) return 'null';
		var v					= [];									// Результат
		var j					= params.shift() || '';					// Разделитель
		var f					= formatters[params.shift()];			// Формат элемента
		if(f==null)
			v					= va;									// Нет формата элемента - возвращаем исходный массив
		else 
			for(var i=0; i<va.length; i++) v.push(f(va[i], params));	// Применяем формат к каждому элементу
		return v.join(j);												// Вернуть результат
	});
})(String.prototype);

// Набор расширений для форматирования чисел:
//   :n                 - вывод числа как есть, если пришла запись типа 1.032434e+34, то вид будет без использования e+xx
//   :n(x,y)            - вывод с дополнением целой части до x символов, дробной части до y символов, дробная часть обрезается до указанного размера
//   :nb(b)             - вывод числа с указанным основанием (для вывода двоичной записи и т.п.)
//   :nf(loc,n1,n2,...) - вывод ед. измерения для числа в нужной форме
// Настройка форматирования:
//   dseparator         - разделитель целой и дробной части
//   nan                - текст, выводимый при попытке отформатировать не число, используется для всех форматов чисел - n, nb, nf
// Инициализация форматирования:
//   String.prototype.format.init({ n: { dseparator: '.', nan: 'NaN' } });
(function(format)
{
	// Регистрируем форматирование
	// Функция форматирования числа с нужным количеством знаков до и после запятой
	format.add('n', function n(v, params)
	{
		if((v = numformat.exec(v))==null) return defaults.nan;	// Уточняем момент насчет числа
		var e					= Number(v[4]);					// Порядок (если указан)
		return isNaN(e)?
					''.concat(v[1], addz(null, v[2], params[0]), addz(defaults.dseparator, v[3], params[1])):	// нет порядка - форматируем, что есть
					shift(v[1], v[2], v[3], e, params[0], params[1]);							// форматирование с учетом порядка
	});
	// Функция форматирования числа в любой системе (от 2-ой до ....)
	format.add('nb', function nb(v, params)
	{
		v						= Number(v);
		if(isNaN(v)) return defaults.nan;
		var b					= Number(params[0]);
		return  addz(null, v.toString(isNaN(b)?16:b), Number(params[1]));
	});
	// Функция вывода ед. измерения для числа в нужной форме
	format.add('nf', function(v, params)
	{
		v						= Number(v);
		if(isNaN(v)) return defaults.nan;
		var f					= nforms[params[0].toLowerCase()];
		return f==null?params[0]+'?':f(v, params);
	});
	// Регистрация функции вычисления формы для нужной локали
	format.get('nf').add		= function(lname, func)
	{
		nforms[lname.toLowerCase()]		= func;
	};
	// Делаем доступным изменение параметров форматирования
	format.get('n').init		= init;
	///////// PRIVATE /////////
	// RegExp для числа
	var numformat				= /^([+-]?)(\d+)(?:\.(\d+))?(?:\s*e([+-]\d+))?$/i;
	// Строка для заполнения нужным количеством нулей
	var zz						= '0000000000';
	// Настройка форматирования:
	// dseparator - разделитель целой и дробной части
	// nan        - текст, выводимый при попытке отформатировать не число
	var defaults				= { dseparator: '.', nan: 'NaN' };
	// Список функций вычисления нужных форм (рубль, рубля, рублей и т.п.)
	var nforms					= {
		en:						function(v, params)
		{
			return params[v==1?1:2];
		},
		ru:						function(v, params)
		{
			var v10				= v%10;
			var v100			= v%100;
			return params[v10==1 && v100!=11?1:v10>=2 && v10<=4 && (v100<10 || v100>=20)?2:3];
		}
	};
	// Функция установки параметров форматирования
	function init(param)
	{
		defaults.dseparator		= param.dseparator || '.';
		defaults.nan			= param.nan || 'NaN';
	}
	// Разбираемся с нужным количеством разрадов
	function addz(pre, v, l)
	{
		if(isNaN(l = Number(l==''?undefined:l))) return (v==null || v=='')?'':((pre || '')+v);	// Нет ограничений - просто вернем то, что есть
		if((v = v || '').length>=l)	return pre==null?v:(pre+v.substr(0, l));		// Значение больше нужного - целую часть оставляем, дробную режем
		return pre==null?(getz(l-v.length)+v):(pre+v+getz(l-v.length));				// Значение меньше нужного - дополняем нулями с соответствующей стороны
	}
	// Получить нужное количество нулей
	function getz(l)
	{
		while(zz.length<l) zz += zz;
		return zz.substring(0, l);
	}
	// Форматирование со сдвигом на нужное кол-во разрядов
	function shift(s, i, f, e, li, lf)
	{
		var m;
		if(e>0)			// перенос из дробной в целую
		{
			m					= addz('', f, e);
			i					+= m;
			f					= f.substr(m.length);
		}else if(e<0)	// перенос из целой в дробную
		{
			m					= i.length-(-e);
			f					= (m>0?i.substring(m, i.length):(getz(-m)+i)) + f;
			i					= (m>0?i.substring(0, -e-1):    '0');
		}
		// Тут нет e==0 - с нулями сюда не ходят!
		return ''.concat(s, addz(null, i, li), addz(defaults.dseparator, f, lf));
	}
})(String.prototype.format);

// Набор расширений для форматирования дат и времени:
//   :df         - используется родное преобразование в строку
//   :df(формат) - используется указанный формат, где нужно указывать:
//                 yyyy или yy - год
//                 M или MM    - месяц   (один или 2 знака)
//                 d или dd    - день    (один или 2 знака)
//                 H или HH    - часы    (один или 2 знака)
//                 m или mm    - минуты  (один или 2 знака)
//                 s или ss    - секунды (один или 2 знака)
//                 f           - миллисекунды (количество букв f - нужное количество знаков)
//   :d          - короткая запись для :df(dd.MM.yyyy)
//   :dt         - короткая запись для :df(dd.MM.yyyy HH:mm:ss)
//   :dt(nosec)  - короткая запись для :df(dd.MM.yyyy HH:mm)
//   :t          - короткая запись для :df(HH:mm:ss)
//   :t(nosec)   - короткая запись для :df(HH:mm)
//   :ts         - вывод числа в виде записи 100 дн. 5 час. 24 мин. 48 сек. 79 мс, нулевые значения не выводятся, параметры:
//          (msec) - до миллисекунд (по-умолчанию)
//           (sec) - до секунд
//           (min) - до минут
//             (h) - до часов
// Инициализация форматирования:
//   Для форматирования даты и времени в строку можно указать короткие записи:
//     String.prototype.format.init({ df: { nad: 'NaD', d: 'dd.MM.yyyy', dts: 'dd.MM.yyyy HH:mm:ss', dt: 'dd.MM.yyyy HH:mm', t: 'HH:mm', ts: 'HH:mm:ss' } });
//   Для форматирования времени счетчика можно указать текст:
//     String.prototype.format.init({ ts: { d: 'дн.', h: 'час.', m: 'мин.', s: 'сек.', ms: 'мс' } });
(function(format)
{
	// Форматирование даты+времени в строку.
	format.add('df', df);
	// Определяем короткую запись для некоторых форматов
	format.add('d',	 function(v, p){ return df(v, intf.d); });
	format.add('dt', function(v, p){ return df(v, p[0]=='nosec'?intf.dt:intf.dts); });
	format.add('t',  function(v, p){ return df(v, p[0]=='nosec'?intf.t :intf.ts ); });
	// Делаем доступным изменение параметров форматирования
	format.get('df').init		= dfinit;

	// Форматирование времени счетчика:
	// работает с объектом Date или целым числом - количеством мсек,
	// выводит строку типа 1дн. 2час. 3мин. 4сек. 5мс, значения, равные нулю пропускаются
	// :ts(min|sec|msec) - на чем остановиться: min - до минут (дни+часы+минуты), sec - до секунд, msec - до мс (по-умолчанию)
	format.add('ts', function ts(v, params)
	{
		if(v==null) return 'null';
		if(v==0)	return '0';
		var s					= [];							// Результат
		var round				= params[0];					// Параметр вывода
		// Пошли обрабатывать... Может переписать с вложением...?
		v						= tss(v, 1000, s, tsdefault.ms, w = (round=='' || round=='msec') );
		v						= tss(v, 60, s, tsdefault.s, w = (w || round=='sec') );
		v						= tss(v, 60, s, tsdefault.m, w = (w || round=='min') );
		v						= tss(v, 24, s, tsdefault.h, true);
		if(v!=0) s.unshift(v, tsdefault.d);						// Не забываем вывести дни, если они есть
		return s.join(' ');										// Результат!
	});
	// Делаем доступным изменение параметров форматирования
	format.get('ts').init		= tsinit;
	
	///////// PRIVATE /////////
	// Кэш функций форматирования даты+время
	var c						= {};
	// Опять строка для заполнения нужным количеством нулей
	var zz						= '0000';
	// Часто используемые форматы
	var intf					= { nad: 'NaD', d: ['dd.MM.yyyy'], dts: ['dd.MM.yyyy HH:mm:ss'], dt: ['dd.MM.yyyy HH:mm'], t: ['HH:mm'], ts: ['HH:mm:ss'] };
	// Строки для форматирования времени счетчика
	var tsdefault					= { d: 'дн.', h: 'час.', m: 'мин.', s: 'сек.', ms: 'мс' };
	// RegExp поиска меток форматирования даты+времени
	var fpre					= /yyyy|yy|m{1,2}|d{1,2}|H{1,2}|M{1,2}|s{1,2}|f{1,4}/g;
	// Чем заменяются метки форматирования
	var fp						= {	y: 'v.getFullYear()', 
									M: 'v.getMonth()+1', 
									d: 'v.getDate()', 
									H: 'v.getHours()', 
									m: 'v.getMinutes()', 
									s: 'v.getSeconds()', 
									f: 'v.getMilliseconds()'
								};
	// Функция инициализации форматирования даты+времени
	function dfinit(param)
	{
		intf.nad				= param.nad || 'NaD';
		intf.d[0]				= param.d   || 'dd.MM.yyyy';
		intf.dts[0]				= param.dts || 'dd.MM.yyyy HH:mm:ss';
		intf.dt[0]				= param.dt  || 'dd.MM.yyyy HH:mm';
		intf.t[0]				= param.t   || 'HH:mm';
		intf.ts[0]				= param.ts  || 'HH:mm:ss';
	}
	// Функция инициализации форматирования времени счетчика
	function tsinit(param)
	{
		tsdefault.d				= param.d  || 'дн.';
		tsdefault.h				= param.h  || 'час.';
		tsdefault.m				= param.m  || 'мин.';
		tsdefault.s				= param.s  || 'сек.';
		tsdefault.ms			= param.ms || 'мс';
	}
	// Функция форматирования
	function df(v, p)
	{
		if(v==null) return 'null';
		if(!(v instanceof Date)) return intf.nad;
		p						= p.join(',');				// Должен остаться один МакКлауд!
		if(p=='') return v;									// Нет формата - делать нечего
		var f					= c[p];						// Пытаемся получить функцию из кэша...
		if(f==null)											// ...облом-с - будем компилировать!
		{	// Строим исходник функции форматирования
			f					= 'return \'\'.concat(\''+
								  (p
									.replace(/'/g, '\\\'')
									.replace(fpre, function(m){
										var mc		= m.charAt(0);
										return mc=='y'?
												''.concat('\', ', fp[mc], ', \''):
												''.concat('\', a(', fp[mc], ', ', m.length, '), \'');
									})
								  )+
								  '\');';
			f					= new Function('v', 'a', f);// Компиляция, так сказать
			c[p]				= f;						// Загоняем в кэш
		}
		return f(v, addz);									// Форматирование!
	}
	// Добавление нужного количества нулей
	function addz(v, l)
	{
		return zz.substring(0, l-(''+v).length)+v;
	}
	// Шаг обработки вывода времени счетчика
	function tss(dt, div, buf, msn, write)
	{
		if(dt==0) return 0;						// Нуль - валим
		var i					= dt % div;		// Остаток от деления
		if(i!=0 && write) buf.unshift(i, msn);	// Выводим, если нужно
		return Math.floor(dt / div);			// Вернем секвестированное число
	}
})(String.prototype.format);
