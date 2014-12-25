using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GST_BLL.Factory
{
   public class DropTable
    {
       String str = @"server=192.168.0.106; database=STAGEDBGSTMASRT; uid=sa; password=Admin123#;";
         SqlConnection con = null;

         public DropTable()
         {
             DropSheet1();
             DropTest();
             DropLedger();
             DropSupply();
             DropPurchase();
         }

         public  bool DropSheet1()
         { 
         
          try
         {
     
            con = new SqlConnection(str);
            con.Open(); //open the connection        
            String cmdText = "  if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Sheet1$')drop table Sheet1$"; // this one drops a table 
            SqlCommand cmd = new SqlCommand(cmdText, con);
            cmd.Prepare();
            cmd.ExecuteNonQuery(); //execute the mysql command
              return true;
         }
         catch (SqlException err)
         {
            String outp = err.ToString();
            Console.WriteLine("Error: " + err.ToString());
             return false;
         }
         finally
         {
            if (con != null)
            {
               con.Close(); //close the connection
            
            }
         } //remember to close the connection after accessing the database





}


       public bool DropTest()
       {
       
       
          try
          {

              con = new SqlConnection(str);
              con.Open(); //open the connection        
              String cmdText = "  if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'test')drop table test"; // this one drops a table 
              SqlCommand cmd = new SqlCommand(cmdText, con);
              cmd.Prepare();
              cmd.ExecuteNonQuery(); //execute the mysql command
              return true;
          }
          catch (SqlException err)
          {
              String outp = err.ToString();
              Console.WriteLine("Error: " + err.ToString());
              return false;
          }
          finally
          {
              if (con != null)
              {
                  con.Close(); //close the connection

              }
          }
      }

       public bool DropLedger()
       {


           try
           {

               con = new SqlConnection(str);
               con.Open(); //open the connection        
               String cmdText = "  if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ledger')drop table ledger"; // this one drops a table 
               SqlCommand cmd = new SqlCommand(cmdText, con);
               cmd.Prepare();
               cmd.ExecuteNonQuery(); //execute the mysql command
               return true;
           }
           catch (SqlException err)
           {
               String outp = err.ToString();
               Console.WriteLine("Error: " + err.ToString());
               return false;
           }
           finally
           {
               if (con != null)
               {
                   con.Close(); //close the connection

               }
           }
       }

       public bool DropSupply()
       {


           try
           {

               con = new SqlConnection(str);
               con.Open(); //open the connection        
               String cmdText = "  if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'supply')drop table supply"; // this one drops a table 
               SqlCommand cmd = new SqlCommand(cmdText, con);
               cmd.Prepare();
               cmd.ExecuteNonQuery(); //execute the mysql command
               return true;
           }
           catch (SqlException err)
           {
               String outp = err.ToString();
               Console.WriteLine("Error: " + err.ToString());
               return false;
           }
           finally
           {
               if (con != null)
               {
                   con.Close(); //close the connection

               }
           }
       }


       public bool DropPurchase()
       {


           try
           {

               con = new SqlConnection(str);
               con.Open(); //open the connection        
               String cmdText = "  if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'purchase')drop table purchase"; // this one drops a table 
               SqlCommand cmd = new SqlCommand(cmdText, con);
               cmd.Prepare();
               cmd.ExecuteNonQuery(); //execute the mysql command
               return true;
           }
           catch (SqlException err)
           {
               String outp = err.ToString();
               Console.WriteLine("Error: " + err.ToString());
               return false;
           }
           finally
           {
               if (con != null)
               {
                   con.Close(); //close the connection

               }
           }
       }  
       
       }


   }