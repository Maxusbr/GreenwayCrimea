<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminUserInformation.ascx.cs" Inherits="AdvantShop.Admin.UserControls.UserInformation.AdminUserInformation" %>

<div hidden>
    <% bool needShow = false;
       if (string.IsNullOrWhiteSpace(Model.CompanyName)
           || string.IsNullOrWhiteSpace(Model.Mobile)
           || string.IsNullOrWhiteSpace(Model.CompanyName))
       {
           needShow |= true;
       }
    %>
    <div id="AdminUserInformation" class="modal-new">
        <div class="modal-new__inner">
            <div class="modal-form">
                <h2 class="modal-form__title">Пожалуйста, ответьте на 6 вопросов для настройки вашего аккаунта</h2>
                <div class="modal-form__fields">
                    <div class="segment-form">
                        <div class="segment-form__inner">
                            <div class="segment-form__field-wrap">
                                <div class="segment-form__field-title">
                                    Ваше имя
                                </div>
                                <div class="segment-form__value-wrap">
                                    <input type="text" name="name" value="<% = Model.Name %>" class="valid-required" autocomplete="off" />
                                    <div class="error-message">
                                        <div class="error-content">Это поле обязательно для заполнения.</div>
                                    </div>
                                    <span class="error-icon error-icon valid-error" style="top: 10px; left: 465px;"></span>
                                </div>

                            </div>
                            <div class="segment-form__field-wrap">
                                <div class="segment-form__field-title">
                                    Ваш номер телефона
                                </div>
                                <div class="segment-form__value-wrap">
                                    <input type="tel" name="phone" class="js-intlTel input-wrap valid-intl-tel" value="<% = Model.Mobile %>" autocomplete="off" />
                                    <div class="error-message">
                                        <div class="error-content">Это поле обязательно для заполнения.</div>
                                    </div>
                                    <span class="error-icon error-icon valid-error" style="top: 10px; left: 465px;"></span>
                                </div>
                            </div>
                            <div class="segment-form__field-wrap">
                                <div class="segment-form__field-title">
                                    Название компании
                                </div>
                                <div class="segment-form__value-wrap">
                                    <input type="text" name="company" value="<% = Model.CompanyName %>" class="valid-required" autocomplete="off" />
                                    <div class="error-message">
                                        <div class="error-content">Это поле обязательно для заполнения.</div>
                                    </div>
                                    <span class="error-icon error-icon valid-error" style="top: 10px; left: 465px;"></span>
                                </div>
                            </div>
                            <div class="segment-form__field-wrap">
                                <div class="segment-form__field-title">
                                    <%  string countUserString = "Количество сотрудников в магазине";
                                        var itemcountUser = Model.Map.Where(x => x.Name == countUserString).Select(x => x.Value).SingleOrDefault();
                                        if (string.IsNullOrWhiteSpace(itemcountUser))
                                            needShow |= true;
                                    %>
                                    <% = countUserString %>
                                </div>
                                <div class="segment-form__value-wrap">
                                    <input class="input-small valid-required" type="number" min="1" name="staff" value="<%=itemcountUser %>" autocomplete="off" autocapitalize="on"/>
                                    <div class="error-message">
                                        <div class="error-content">Это поле обязательно для заполнения.</div>
                                    </div>
                                    <span class="error-icon error-icon valid-error" style="top: 10px; left: 465px;"></span>
                                </div>
                            </div>
                            <div class="segment-form__field-wrap">
                                <div class="segment-form__field-title">
                                    <%
                                        string q2 = "Ваш опыт продаж в интернете?";
                                        var itemQ2 = Model.Map.Where(x => x.Name == q2).Select(x => x.Value).FirstOrDefault();
                                        var valuesQ2 = new List<string> { "Еще не продавали", "Продавали в соц.сетях", "Делали рекламу в интернете", "Был/Есть интернет-магазин" };
                                        if (itemQ2 != null && !valuesQ2.Contains(itemQ2))
                                            needShow |= true;
                                    %>
                                    <% =q2%>
                                </div>
                                <div class="segment-form__wrap">
                                    <select class="segment-form__select valid-select" name="segment-exp">
                                        <option selected="selected" value="" disabled="disabled">...</option>
                                        <option <% =(itemQ2 == "Еще не продавали" ? "selected=\"selected\"" : "")%>>Еще не продавали</option>
                                        <option <% =(itemQ2 == "Продавали в соц.сетях" ? "selected=\"selected\"" : "")%>>Продавали в соц.сетях</option>
                                        <option <% =(itemQ2 == "Делали рекламу в интернете" ? "selected=\"selected\"" : "")%>>Делали рекламу в интернете</option>
                                        <option <% =(itemQ2 == "Был/Есть интернет-магазин" ? "selected=\"selected\"" : "")%>>Был/Есть интернет-магазин</option>
                                    </select>
                                    <div class="error-message">
                                        <div class="error-content">Это поле обязательно для заполнения.</div>
                                    </div>
                                    <span class="error-icon error-icon valid-error" style="top: 10px; left: 465px;"></span>
                                </div>
                            </div>
                            <div class="segment-form__field-wrap">
                                <div class="segment-form__field-title">
                                    <% 
                                        string q3 = "Есть ли у вас точки продаж в офлайне?";
                                        var itemQ3 = Model.Map.Where(x => x.Name == q3).Select(x => x.Value).FirstOrDefault();

                                        var valuesQ3 = new List<string> { "Есть розница", "Есть оптовые", "Оптовые и розничные", "Пока нет" };
                                        if (itemQ3 != null && !valuesQ3.Contains(itemQ3))
                                            needShow |= true;
                                    %>
                                    <% = q3 %>
                                </div>
                                <div class="segment-form__wrap">
                                    <select class="segment-form__select valid-select valid-required" name="segment-offline">
                                        <option selected="selected" value="" disabled="disabled">...</option>
                                        <option <% =(itemQ3 == "Есть розница" ? "selected=\"selected\"" : "")%>>Есть розница</option>
                                        <option <% =(itemQ3 == "Есть оптовые" ? "selected=\"selected\"" : "")%>>Есть оптовые</option>
                                        <option <% =(itemQ3 == "Оптовые и розничные" ? "selected=\"selected\"" : "")%>>Оптовые и розничные</option>
                                        <option <% =(itemQ3 == "Пока нет" ? "selected=\"selected\"" : "")%>>Пока нет</option>
                                    </select>
                                    <div class="error-message">
                                        <div class="error-content">Это поле обязательно для заполнения.</div>
                                    </div>
                                    <span class="error-icon error-icon valid-error" style="top: 10px; left: 465px;"></span>
                                </div>
                            </div>
                        </div>
                        <div class="segment-form__btn-wrap">
                            <button class="segment-form-btn">Закончить регистрацию</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-info">
                <div class="modal-info__inner">
                    <h4 class="modal-info__title">Зачем это нужно?</h4>
                    <div class="modal-info__text">Пожалуйста, ответьте на все вопросы. Эти данные помогут нам лучше понимать ваши потребности и предложить вам лучшее решение для работы интернет-магазина. Ваш телефон поможет быстрее решать возникшие у вас вопросы при обращении в службу поддержки.</div>
                    <div class="modal-info__about">
                        <div class="modal-info__about-block">
                            <div class="modal-info__feature calendar">14 дней бесплатно</div>
                        </div>
                        <div class="modal-info__about-block">
                            <div class="modal-info__feature wing">Полный функционал без ограничений</div>
                        </div>
                        <div class="modal-info__about-block modal-info__about-block--margin-bot-none">
                            <div class="modal-info__feature shield">Ваши данные надежно защищены</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" class="needShow" value="<% = needShow %>" />
    </div>
</div>
