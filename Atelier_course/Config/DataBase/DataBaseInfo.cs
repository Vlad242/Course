namespace Atelier_course.Config.DataBase
{
    class DataBaseInfo
    {
        private string serverName = "localhost";
        private string userName = "root";
        private string userPassword = "masterkey";
        private string charset = "utf8";
        private string databaseName = "DB_Atelier";
        private string port = "3306";

        public string ServerName
        {
            get => serverName;
            set => serverName = value;
        }
        public string UserName
        {
            get => userName;
            set => userName = value;
        }
        public string UserPassword
        {
            get => userPassword;
            set => userPassword = value;
        }
        public string Charset
        {
            get => charset;
            set => charset = value;
        }
        public string DatabaseName
        {
            get => databaseName;
            set => databaseName = value;
        }
        public string Port
        {
            get => port;
            set => port = value;
        }

        public string GetConnectInfo()
        {
            return "server=" + this.serverName + ";user=" + this.userName +
                   ";charset=" + this.charset + ";database=" + this.databaseName +
                   ";port=" + this.port + ";password=" + this.userPassword + ";";
        }
    }
}
