using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Atelier_course.Mailer
{
    class Generator
    {
        public MySqlConnection conn;

        private void SetConnection()
        {
            Config.DataBase.DataBaseInfo dataBase = new Config.DataBase.DataBaseInfo();
            conn = new MySqlConnection(dataBase.GetConnectInfo());
            conn.Open();
        }

        private void CloseCOnnection()
        {
            conn.Close();
        }

        public string GenerateNewOrderBody(int order_id, string ComandName)
        {
            string result = "";
            Config.Mailer.MailerConfig mailer = new Config.Mailer.MailerConfig();
            string mail = mailer.userName;
            ////////////USER INFO
            string user_surname = "";
            string user_name = "";
            string user_fname = "";
            ///////////////ORDER INFO
            string Service_type = "";
            string Service_name = "";
            string Price = "";
            string Status_name = "";
            string Order_date = "";

            string Address = "";

            try
            {
                SetConnection();

                ///USER INFO
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Office.Address FROM Users INNER JOIN Office on(Users.Office_id=Office.Office_id) WHERE Users.User_id=(SELECT Worker_id FROM Orders WHERE Order_id= " +order_id+ ");")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Address = reader.GetString(0);
                }
                reader.Close();
                //////////////ORDER INFO
                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Service_type.Type_name, Services.Service_name, Orders.Price, Status.Status_name, Orders.Order_date, Users.U_surname, Users.U_name, Users.U_fname FROM Orders INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Status on(Status.Status_id=Orders.Status_id) INNER JOIN Service_type on(Service_type.Type_id=Services.Type_id) INNER JOIN Users on(Users.User_id=Orders.User_id) WHERE Orders.Order_id=" + order_id + ";")
                };
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Service_type = reader.GetString(0);
                    Service_name = reader.GetString(1);
                    Price = reader.GetString(2);
                    Status_name = reader.GetString(3);
                    Order_date = reader.GetString(4).Remove(10);

                    user_surname = reader.GetString(5);
                    user_name = reader.GetString(6);
                    user_fname = reader.GetString(7);
                }
                reader.Close();
                ///////////GET RESULT

                result = "Dear user " + user_surname + " " + user_name + " " +user_fname + "." + "<br>" + /*< font size = "5" color = "red" face = "Arial" > П </ font >*/
                                     "Your order #" + order_id + " is registered in the system base, the status of the order - " + Status_name + "." + "<br>" +
                                "Order detail:" + "<br>" +
                                "Service: " + Service_type + " " + Service_name  + ";" + "<br>" +
                                "Price: " + Price + ";" + "<br>" +
                                "Order date: " + Order_date + ";" + "<br>" + "<br>" +
                                "For more information, please visit our office at: " + Address + "<br>" +
                    "Email: " + mail + ";" + "<br>" +
                    "Phone for help: 937-99-92" + "<br>" + "<br>" +
                    "Sincerely team " + ComandName + " ! :)";

            }
            catch (Exception ex)
            {

            }
            CloseCOnnection();
            return result;
        }

        public string GenerateActiveBody(string detail, int order_id, string ComandName)
        {
            string result = "";
            Config.Mailer.MailerConfig mailer = new Config.Mailer.MailerConfig();
            string mail = mailer.userName;
            ////////////USER INFO
            string user_surname = "";
            string user_name = "";
            string user_fname = "";
            ///////////////ORDER INFO
            string Service_type = "";
            string Service_name = "";
            string Status_name = "";
            string Order_date = "";

            string Address = "";

            try
            {
                SetConnection();

                ///USER INFO
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Office.Address FROM Users INNER JOIN Office on(Users.Office_id=Office.Office_id) WHERE Users.User_id=(SELECT Worker_id FROM Orders WHERE Order_id= " + order_id + ");")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Address = reader.GetString(0);
                }
                reader.Close();
                //////////////ORDER INFO
                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Service_type.Type_name, Services.Service_name, Status.Status_name, Orders.Order_date, Users.U_surname, Users.U_name, Users.U_fname FROM Orders INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Status on(Status.Status_id=Orders.Status_id) INNER JOIN Service_type on(Service_type.Type_id=Services.Type_id) INNER JOIN Users on(Users.User_id=Orders.User_id) WHERE Orders.Order_id=" + order_id + ";")
                };
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Service_type = reader.GetString(0);
                    Service_name = reader.GetString(1);
                    Status_name = reader.GetString(2);
                    Order_date = reader.GetString(3).Remove(10);

                    user_surname = reader.GetString(4);
                    user_name = reader.GetString(5);
                    user_fname = reader.GetString(6);
                }
                reader.Close();
                ///////////GET RESULT

                result = "Dear user " + user_surname + " " + user_name + " " + user_fname + "." + "<br>" + /*< font size = "5" color = "red" face = "Arial" > П </ font >*/
                                     "Your order #" + order_id + " has been reviewed, the current status - " + Status_name + "." + "<br>" +
                                "Order detail:" + "<br>" +
                                "Service: " + Service_type + " " + Service_name + ";" + "<br>" +
                                "Addons: " + detail + "<br>" +
                                "Order date: " + Order_date + ";" + "<br>" + "<br>" +
                                "For more information, please visit our office at: " + Address + "<br>" +
                    "Email: " + mail + ";" + "<br>" +
                    "Phone for help: 937-99-92" + "<br>" + "<br>" +
                    "Sincerely team " + ComandName + " ! :)";

            }
            catch (Exception ex)
            {

            }
            CloseCOnnection();
            return result;
        }

        public string GenerateCompleteOrderBody(int order_id, string ComandName)
        {
            string result = "";
            Config.Mailer.MailerConfig mailer = new Config.Mailer.MailerConfig();
            string mail = mailer.userName;
            ////////////USER INFO
            string user_surname = "";
            string user_name = "";
            string user_fname = "";
            ///////////////ORDER INFO
            string Service_type = "";
            string Service_name = "";
            string Price = "";
            string Order_date = "";

            string Address = "";

            try
            {
                SetConnection();

                ///USER INFO
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Office.Address FROM Users INNER JOIN Office on(Users.Office_id=Office.Office_id) WHERE Users.User_id=(SELECT Worker_id FROM Orders WHERE Order_id= " + order_id + ");")
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Address = reader.GetString(0);
                }
                reader.Close();
                //////////////ORDER INFO
                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = string.Format("SELECT Service_type.Type_name, Services.Service_name, Orders.Price, Orders.Order_date, Users.U_surname, Users.U_name, Users.U_fname FROM Orders INNER JOIN Services on(Services.Service_id=Orders.Service_id) INNER JOIN Status on(Status.Status_id=Orders.Status_id) INNER JOIN Service_type on(Service_type.Type_id=Services.Type_id) INNER JOIN Users on(Users.User_id=Orders.User_id) WHERE Orders.Order_id=" + order_id + ";")
                };
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Service_type = reader.GetString(0);
                    Service_name = reader.GetString(1);
                    Price = reader.GetString(2);
                    Order_date = reader.GetString(3).Remove(10);

                    user_surname = reader.GetString(4);
                    user_name = reader.GetString(5);
                    user_fname = reader.GetString(6);
                }
                reader.Close();
                ///////////GET RESULT

                result = "Dear user " + user_surname + " " + user_name + " " + user_fname + "." + "<br>" + /*< font size = "5" color = "red" face = "Arial" > П </ font >*/
                                     "Your order #" + order_id + " Complete!" + "<br>" +
                                "Order detail:" + "<br>" +
                                "Service: " + Service_type + " " + Service_name + ";" + "<br>" +
                                "Price: " + Price + ";" + "<br>" +
                                "Order date: " + Order_date + ";" + "<br>" + "<br>" +
                                "For more information, please visit our office at: " + Address + "<br>" +
                    "Email: " + mail + ";" + "<br>" +
                    "Phone for help: 937-99-92" + "<br>" + "<br>" +
                    "Sincerely team " + ComandName + " ! :)";

            }
            catch (Exception ex)
            {

            }
            CloseCOnnection();
            return result;
        }
        public string GenerateSubject(string CompanyName, int orderId)
        {
            return CompanyName + " order number " + orderId;
        }
    }
}
