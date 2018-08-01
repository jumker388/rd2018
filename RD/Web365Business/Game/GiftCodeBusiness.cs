using System.Data.SqlClient;
using System.IO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Business.Game
{
    public class GiftCodeBusiness
    {

        private static Random random = new Random();
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public List<GiftCodeItem> Generator(string name, DateTime dateExpired, int Gift_ID, string prefix, string note, string giatri, string soluong)
        {

            List<int> Lstsoluong = soluong.Split(',').Select(Int32.Parse).ToList();
            int[] Arrgiatri = giatri.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            int[] Arrsoluong = soluong.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            var notefix = "";

            for (int i = 0; i < Arrgiatri.Length; i++)
            {
                notefix += Arrsoluong[i] + " mã trị giá : " + Arrgiatri[i] + ", ";
            }

            int total = Lstsoluong.Sum(c=>c);
            
            int value = 0;

            var turnID = 0;
            var conn = new SqlConnection(Constants.DBConnection);
            conn.Open();

            var lstTurn = new GiftCodeItem();
            string sqlTurn = "insert into [newDB].[dbo].[gift_turn](Name, [Quantity],[Prefix],[DateExpired],[IsShow] ,[IsDelete],[TypeID],[Note],DateCreated, NoteFix) OUTPUT INSERTED.ID VALUES(@Name, @Quantity,@Prefix,@DateExpired,1,0,@TypeID,@Note,GETDATE(), @NoteFix)";
            var dateCreated = DateTime.Now;
            var MyCommand = new SqlCommand(sqlTurn, conn);
            MyCommand.Parameters.Clear();
            MyCommand.Parameters.AddWithValue("Quantity", total);
            MyCommand.Parameters.AddWithValue("Prefix", prefix);
            MyCommand.Parameters.AddWithValue("DateExpired", dateExpired.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("TypeID", Gift_ID);
            MyCommand.Parameters.AddWithValue("Note", note);
            MyCommand.Parameters.AddWithValue("Name", name);
            MyCommand.Parameters.AddWithValue("NoteFix", notefix); 
            turnID = (int)MyCommand.ExecuteScalar();

            var lst = new List<GiftCodeItem>();

            for (int j = 0; j < Arrsoluong.Length; j++)
            {
                for (int i = 1; i <= Arrsoluong[j]; i++)
                {
                    string sql = "insert into [newDB].[dbo].giftcode(Code, DateCreated,DateExpired, TurnID, Gift_ID, IsDelete, Money) OUTPUT INSERTED.id values('" + prefix.ToUpper() + RandomString(10) + "', GetDate(), @dateExpired, @turnID, @Gift_ID, 0, @Money)";
                    var myCommand = new SqlCommand(sql, conn);
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("value", value);
                    myCommand.Parameters.AddWithValue("dateExpired", dateExpired.ToString(Constants.DateFormat));
                    myCommand.Parameters.AddWithValue("Gift_ID", Gift_ID);
                    myCommand.Parameters.AddWithValue("turnID", turnID);
                    myCommand.Parameters.AddWithValue("Money", Convert.ToDecimal(Arrgiatri[j]));
                    var inserted = (int)myCommand.ExecuteScalar();
                    if (inserted > 0)
                    {
                        var item = new GiftCodeItem
                        {
                            ID = inserted
                        };
                        lst.Add(item);
                    }
                }
            }



            conn.Close();
            return lst;
        }

        public GiftCodeItem InsertOneItem(string codeIn, int value, string name, DateTime dateExpired,int isVCash)
        {
            var lst = new GiftCodeItem();
            string sql = "insert into [newDB].[dbo].giftcode(Code, [Values], DateCreated, DateExpired, Gift_ID, IsDelete) OUTPUT INSERTED.id values('" + codeIn + "', @value, GetDate(), @dateExpired, @Gift_ID, 0)";
            var dateCreated = DateTime.Now;
            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            conn.Open();
            MyCommand.Parameters.Clear();
            MyCommand.Parameters.AddWithValue("value", value);
            MyCommand.Parameters.AddWithValue("dateExpired", dateExpired.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("Gift_ID", isVCash);
            var inserted = (int)MyCommand.ExecuteScalar();
            if (inserted > 0)
            {
                var item = new GiftCodeItem
                {
                    ID = inserted
                };
                lst = item;
            }

            conn.Close();
            return lst;
        }

        private string GenGiftCode()
        {
            var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public GiftCodeResult GetAll(bool used, int skip, int take)
        {
            var rs = new GiftCodeResult();
            List<GiftCodeItem> data = new List<GiftCodeItem>();
            int totalRecord = 0;
            string sql = "";

            if (used)
            {
                if (used == true)
                {
                    sql = "SELECT u.username, gc.* FROM [newDB].[dbo].giftcode gc inner join [newDB].[dbo].g_user u on gc.user_id = u.user_id WHERE gc.user_id > 0 ORDER BY gc.id DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";   
                }
                else
                {
                    sql = "SELECT u.username, gc.* FROM [newDB].[dbo].giftcode gc inner join [newDB].[dbo].g_user u on gc.user_id = u.user_id ORDER BY gc.id DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";   
                }
                SqlConnection conn = new SqlConnection(Constants.DBConnection);
                SqlCommand MyCommand = new SqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("skip", skip);
                MyCommand.Parameters.AddWithValue("take", take);
                SqlDataReader MyReader;
                conn.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    var item = new GiftCodeItem
                    {
                        code = MyReader.GetString(MyReader.GetOrdinal("code")),
                        ID = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                        dateCreated = MyReader.GetDateTime(MyReader.GetOrdinal("dateCreated")),
                        dateExpired = MyReader.GetDateTime(MyReader.GetOrdinal("dateExpired")),
                        name = MyReader.GetString(MyReader.GetOrdinal("name")),
                        value = MyReader.GetInt32(MyReader.GetOrdinal("value")),
                        user_id = MyReader.GetInt32(MyReader.GetOrdinal("user_id")),
                        username = MyReader.GetString(MyReader.GetOrdinal("username")),
                        fullname = MyReader.GetString(MyReader.GetOrdinal("fullname")),
                        isVCash = MyReader.GetInt32(MyReader.GetOrdinal("isVCash"))
                        
                    };
                    item.sdateCreated = item.dateCreated.ToShortDateString();
                    item.sdateExpired = item.dateExpired.ToShortDateString();
                    data.Add(item);
                }
                MyReader.Close();
                sql = "Select count(*) as totalRecord from [portal].[dbo].giftcode WHERE used = 1";
                MyCommand = new SqlCommand(sql, conn);
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    totalRecord = MyReader.GetInt32(MyReader.GetOrdinal("totalRecord"));
                }
                MyReader.Close();
                conn.Close();
            }
            else
            {
                sql = "SELECT * FROM [portal].[dbo].giftcode WHERE used = 0 ORDER BY dateCreated DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";
                SqlConnection conn = new SqlConnection(Constants.DBConnection);
                SqlCommand MyCommand = new SqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("skip", skip);
                MyCommand.Parameters.AddWithValue("take", take);
                SqlDataReader MyReader;
                conn.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    GiftCodeItem item = new GiftCodeItem();
                    item.code = MyReader.GetString(MyReader.GetOrdinal("code"));
                    item.ID = MyReader.GetInt32(MyReader.GetOrdinal("id"));
                    item.dateCreated = MyReader.GetDateTime(MyReader.GetOrdinal("dateCreated"));
                    item.dateExpired = MyReader.GetDateTime(MyReader.GetOrdinal("dateExpired")); ;
                    item.used = false;
                    item.name = MyReader.GetString(MyReader.GetOrdinal("name"));
                    item.value = MyReader.GetInt32(MyReader.GetOrdinal("value"));
                    item.isVCash = MyReader.GetInt32(MyReader.GetOrdinal("isVCash"));
                    item.user_id = 0;
                    item.username = "";
                    item.fullname = "";
                    item.sdateCreated = item.dateCreated.ToShortDateString();
                    item.sdateExpired = item.dateExpired.ToShortDateString();
                    data.Add(item);
                }
                MyReader.Close();
                sql = "Select count(*) as totalRecord from [portal].[dbo].giftcode WHERE used = 0";
                MyCommand = new SqlCommand(sql, conn);
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    totalRecord = MyReader.GetInt32(MyReader.GetOrdinal("totalRecord"));
                }
                MyReader.Close();
                conn.Close();
            }
            rs.totalRecord = totalRecord;
            rs.data = data;
            return rs;
        }
    }
}
