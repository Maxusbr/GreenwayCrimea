


INSERT INTO	[Settings].[MailFormatType] ([TypeName],[SortOrder], [Comment])
VALUES  ('Уведомление об ответе на отзыв', 210, 'Уведомление пользователю с ответом на его отзыв о товаре (#PRODUCTNAME#; #PRODUCTLINK#; #SKU#; #AUTHOR#; #DATE#; #ANSWER_TEXT#, #PREVIOUS_MSG_TEXT#)')

GO--

INSERT INTO [Settings].[MailFormat]
        ( [FormatName] ,
          [FormatText] ,
          [FormatType] ,
          [SortOrder] ,
          [Enable] ,
          [AddDate] ,
          [ModifyDate] ,
          [FormatSubject]
        )
VALUES  ( N'Уведомление об ответе на отзыв',
          N'<div style="color: #4c4f56; font-family: Arial, Helvetica, sans-serif; font-size: 14px;"><div class="header" style="border-bottom: 1px solid #ededed; display: table; margin-bottom: 25px; padding-bottom: 25px; width: 100%;"> <div class="logo" style="display: table-cell; text-align: left; vertical-align: middle;">#LOGO#</div> <div class="phone" style="display: table-cell; text-align: right; vertical-align: middle;"> <div class="tel" style="font-size: 26px; font-weight: bold; line-height: 1; margin-bottom: 5px;">&nbsp;</div> <div class="inform" style="font-size: 12px;">&nbsp;</div> </div> </div> <p>Пользователь #AUTHOR# ответил на Ваш отзыв о продукте <a href="#PRODUCTLINK#" title="#PRODUCTNAME#">#PRODUCTNAME#</a>:</p> <div style="padding:5px 10px;background-color:rgb(243, 243, 243)">&gt;#PREVIOUS_MSG_TEXT#</div> <p>#ANSWER_TEXT#</p> <p><a href="#PRODUCTLINK#" title="#PRODUCTNAME#">Ответить на комментарий</a></p> </div>',
          22,
          1100,
          1,
          GETDATE(),
          GETDATE(),
          N'Новый комментарий к #PRODUCTNAME#'
        )

GO--