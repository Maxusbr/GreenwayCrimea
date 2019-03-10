namespace AdvantShop.Module.Roistat.Models.Roistat
{
    public class RoistatOrdersExportModel
    {
        /// <summary>
        /// Дата (в формате UNIX-time) после которой, были изменения в сделках
        /// </summary>
        public double Date { get; set; }

        /// <summary>
        /// Имя пользователя (Указывается в настройках интеграции)
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// md5 ( $username . $password )
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Лимит количества выгружаемых сделок
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Лимит количества выгружаемых сделок
        /// </summary>
        public int Offset { get; set; }
    }
}
