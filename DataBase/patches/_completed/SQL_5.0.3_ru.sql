
Update [Settings].[MailFormat] Set [FormatText] = Replace([FormatText], '+7 (800) 333-55-77', '#MAIN_PHONE#')  Where FormatType = 3; 
Update [Settings].[MailFormat] Set [FormatText] = Replace([FormatText], '<div class="inform" style="font-size: 12px;">Ежедневно с 9:00 до 20:00</div>', '')  Where FormatType = 3; 
GO--


