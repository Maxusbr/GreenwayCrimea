
Update [Settings].[MailFormatType] Set Comment = Replace(Comment, '#TEXTMESSAGE#', '#TEXTMESSAGE#; #ORDERNUMBER#') Where MailFormatTypeId = 8
GO--

Update [Settings].[MailFormat] Set FormatText = Replace(FormatText, '#USERPHONE#</div>', '#USERPHONE#</div>  </div> <div class="l-row">  <div class="l-name vi cs-light" style="color: #acacac; display: inline-block; margin: 5px 0; padding-right: 15px; width: 80px; vertical-align: middle;">Номер заказа:</div>  <div class="l-value vi" style="display: inline-block; margin: 5px 0;">#ORDERNUMBER#</div>') Where FormatType = 8
GO--

UPDATE [Settings].[InternalSettings] SET [settingValue] = '5.0.8' WHERE [settingKey] = 'db_version'


