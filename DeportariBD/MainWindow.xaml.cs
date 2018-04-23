using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeportariBD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string conn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='c:\users\cristian\documents\visual studio 2015\Projects\DeportariBD\DeportariBD\BazaDate.mdf';Integrated Security=True";
        int state = 1;
        string selid = "";
        public MainWindow()
        {
            InitializeComponent();

            setVisibilityComponents();

            button_Details.Visibility = System.Windows.Visibility.Hidden;
            button_Delete_Selection.Visibility = System.Windows.Visibility.Hidden;
            textBox_Update.Visibility = System.Windows.Visibility.Hidden;
            button_update.Visibility = System.Windows.Visibility.Hidden;
            listBox1.Visibility = System.Windows.Visibility.Hidden;
            scrollViewer1.Visibility = System.Windows.Visibility.Hidden;

            comboBox.SelectedIndex = 0;

        }
        public void setVisibilityComponents()
        {
            add_button.Visibility = System.Windows.Visibility.Hidden;
            comboBox_Deportat_Adder.Visibility = System.Windows.Visibility.Hidden;
            textBox_Deportat_Name.Visibility = System.Windows.Visibility.Hidden;
            textBox_Deportat_Prenume.Visibility = System.Windows.Visibility.Hidden;
            textBox_Deportat_Data_nasterii.Visibility = System.Windows.Visibility.Hidden;
            textBox_Deportat_Data_Decesului.Visibility = System.Windows.Visibility.Hidden;
            comboBox_Loc_Deportare_adder.Visibility = System.Windows.Visibility.Hidden;
            comboBox_Loc_Reabilitare_adder.Visibility = System.Windows.Visibility.Hidden;
            textBox_Data_Deportare.Visibility = System.Windows.Visibility.Hidden;
            textBox_Localitate.Visibility = System.Windows.Visibility.Hidden;
            textBox_Localitate_Regiune.Visibility = System.Windows.Visibility.Hidden;
            textBox_Obiect.Visibility = System.Windows.Visibility.Hidden;
            textBox_Obiect_Utilizabil.Visibility = System.Windows.Visibility.Hidden;
            textBox_Data_Reabilitare.Visibility = System.Windows.Visibility.Hidden;
            textBox_Regiune.Visibility = System.Windows.Visibility.Hidden;
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
        }

        private void button_Click_Search(object sender, RoutedEventArgs e)
        {
            state = 1;
            bool justone = false;
            SqlConnection con = new SqlConnection(conn);
            con.Open();

            string command = "select left(ID,5), left(NUME+replicate(' ',10),10),left(PRENUME+replicate(' ',10),10),DATA_NASTERII, DATA_DECES, (select DENUMIRE from Localitati where ID_LOCALITATE=ID_LOCALITATE_RESEDINTA) from Deportati";
            if (textBox_Search_Nume.Text != "" || textBox_Search_Prenume.Text != "" || textBox_Search_Data_Nasterii.Text != ""
                || textBox_Search_Data_Deportarii.Text != "" || textBox_Search_Data_Reabilitarii.Text != ""
                || textBox_Search_Data_Deces.Text != "" || textBox_Search_Loc_Nastere.Text != ""
                || textBox_Search_Loc_Reabilitare.Text != "" || textBox_Search_Loc_Deportare.Text != ""
                || textBox_Search_Confiscari.Text != "")
                command += " where";
            if (textBox_Search_Nume.Text != "")
            {
                command += " lower(Nume) LIKE lower('%" + textBox_Search_Nume.Text + "%')";
                justone = true;
            }

            if (textBox_Search_Prenume.Text != "")
            {
                command = justone == true ? command + " and" : command;
                justone = false;
                command += " lower(Prenume) LIKE lower('%" + textBox_Search_Prenume.Text + "%')";
                justone = true;
            }

            if (textBox_Search_Data_Nasterii.Text != "")
            {
                command = justone == true ? command + " and" : command;
                justone = false;
                command += " DATA_NASTERII=convert(date, '" + textBox_Search_Data_Nasterii.Text + "',104)";
                justone = true;
            }
            if (textBox_Search_Data_Deces.Text != "")
            {
                command = justone == true ? command + " and" : command;
                justone = false;
                command += " DATA_DECES=convert(date, '" + textBox_Search_Data_Deces.Text + "',104)";
                justone = true;
            }
            if (textBox_Search_Data_Deportarii.Text != "")
            {
                command = justone == true ? command + " and" : command;
                justone = false;
                command += "  (select DATA_DEPORTARE from DETALIIDEPORTARE where ID_DEPORTAT=ID)=convert(date, '" + textBox_Search_Data_Deportarii.Text + "',104)";
                justone = true;
            }
            if (textBox_Search_Data_Reabilitarii.Text != "")
            {
                command = justone == true ? command + " and" : command;
                justone = false;
                command += "  (select DATA_REABILITARE from DETALIIDEPORTARE where ID_DEPORTAT=ID)=convert(date, '" + textBox_Search_Data_Reabilitarii.Text + "',104)";
                justone = true;
            }
            if (textBox_Search_Loc_Nastere.Text != "")
            {
                command = justone == true ? command + " and" : command;
                justone = false;
                command += "  ID_LOCALITATE_RESEDINTA = (select ID_LOCALITATE from LOCALITATI where lower(DENUMIRE) like lower('%" + textBox_Search_Loc_Nastere.Text + "%'))";
                justone = true;
            }
            if (textBox_Search_Loc_Deportare.Text != "")
            {
                command = justone == true ? command + " and" : command;
                justone = false;
                command += "  (select ID_LOCALITATE_DEPORTARE from DETALIIDEPORTARE where ID_DEPORTAT=ID) = (select ID_LOCALITATE from LOCALITATI where lower(DENUMIRE) like lower('%" + textBox_Search_Loc_Deportare.Text + "%'))";
                justone = true;
            }
            if (textBox_Search_Loc_Reabilitare.Text != "")
            {
                command = justone == true ? command + " and" : command;
                justone = false;
                command += "  (select ID_LOCALITATE_REABILITARE from DETALIIDEPORTARE where ID_DEPORTAT=ID) = (select ID_LOCALITATE from LOCALITATI where lower(DENUMIRE) like lower('%" + textBox_Search_Loc_Reabilitare.Text + "%'))";
                justone = true;
            }
            if (textBox_Search_Confiscari.Text != "")
            {
                command = justone == true ? command + " and" : command;
                justone = false;
                command += "  ID=(select ID_DEPORTAT from DETALIICONFISCARI where ID_OBIECT=(select ID from OBIECTE where lower(DENUMIRE) like lower('%" + textBox_Search_Confiscari.Text + "%') ))";
                justone = true;
            }

            listBox1.Items.Clear();


            SqlCommand cmd = new SqlCommand(command, con);

            SqlDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                textBlock_Logger.Text = "Succes";
            }
            catch (SqlException exc)
            {
                textBlock_Logger.Text = exc.Message;
            }

            listBox1.FontFamily = new FontFamily("Courier New");

            listBox1.Items.Add("ID  Nume       Prenume    Data Nasterii   Data Decesului    Localitare Resedinta");
            try
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(4))
                        listBox1.Items.Add(rdr.GetString(0).PadRight(3, ' ') + ' ' + rdr.GetString(1) + ' ' + rdr.GetString(2) + ' ' + rdr.GetDateTime(3).ToShortDateString() + "      " + rdr.GetDateTime(4).ToShortDateString() + "        " + rdr.GetString(5));
                    else
                    {
                        listBox1.Items.Add(rdr.GetString(0).PadRight(3, ' ') + ' ' + rdr.GetString(1) + ' ' + rdr.GetString(2) + ' ' + rdr.GetDateTime(3).ToShortDateString() + "      NULL              " + rdr.GetString(5));

                    }
                }
                textBlock_Logger.Text = "Succes";
            }
            catch (SqlException exc)
            {
                textBlock_Logger.Text = exc.Message;
            }

            // Setting the visibility components


            textBox_Update.Visibility = System.Windows.Visibility.Hidden;//hiding update buttons bc we are in state 1
            button_update.Visibility = System.Windows.Visibility.Hidden;
            button_Details.Visibility = System.Windows.Visibility.Visible;//showing viewDetails button
            if (listBox1.Items.IsEmpty)//if listbox is empty, hide all controlls
            {
                listBox1.Visibility = System.Windows.Visibility.Hidden;
                scrollViewer1.Visibility = System.Windows.Visibility.Hidden;
                button_Details.Visibility = System.Windows.Visibility.Hidden;
                button_Delete_Selection.Visibility = System.Windows.Visibility.Hidden;
                textBox_Update.Visibility = System.Windows.Visibility.Hidden;
                button_update.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                scrollViewer1.Visibility = System.Windows.Visibility.Visible;
                listBox1.Visibility = System.Windows.Visibility.Visible;
                button_Details.Visibility = System.Windows.Visibility.Visible;
                button_Delete_Selection.Visibility = System.Windows.Visibility.Visible;
            }
            rdr.Close();
            con.Close();
        }

        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /* brief: Starea 1: sterge detalii deportat si deportat selectat
         *        Starea 2: sterge detalii deportat selectat
         */
        private void button_Delete_Selection_Click(object sender, RoutedEventArgs e)
        {
            if (state == 1)
            {
                if (listBox1.SelectedIndex != -1 && listBox1.SelectedIndex != 0)
                {
                    string deleteitem = listBox1.SelectedItem.ToString();
                    deleteitem = deleteitem.Split(' ')[0];
                    SqlConnection con = new SqlConnection(conn);
                    con.Open();

                    string command = "delete from DETALIIDEPORTARE where ID_DEPORTAT=" + deleteitem;
                    SqlCommand cmd = new SqlCommand(command, con);
                    SqlDataReader rdr = null;
                    try
                    {
                        rdr = cmd.ExecuteReader();
                        textBlock_Logger.Text = "Succes";
                    }
                    catch (SqlException exc)
                    {
                        textBlock_Logger.Text = exc.Message;
                    }
                    rdr.Close();

                    command = "delete from DEPORTATI where ID=" + deleteitem;
                    cmd = new SqlCommand(command, con);
                    try
                    {
                        rdr = cmd.ExecuteReader();
                        textBlock_Logger.Text = "Succes";
                    }
                    catch (SqlException exc)
                    {
                        textBlock_Logger.Text = exc.Message;
                    }
                    rdr.Close();

                    con.Close();
                    button_Click_Search(null, null);
                }
            }
            else if (state == 2)
            {
                string command = "";
                string MAIN_ID = listBox1.Items.GetItemAt(0).ToString().Split(':')[1];
                SqlConnection con = new SqlConnection(conn);
                con.Open();

                command = "delete from DETALIIDEPORTARE where ID_DEPORTAT=" + MAIN_ID;
                SqlCommand cmd = new SqlCommand(command, con);
                SqlDataReader rdr;
                try
                {
                    rdr = cmd.ExecuteReader();
                    textBlock_Logger.Text = "Succes";
                    rdr.Close();
                }
                catch (SqlException exc)
                {
                    textBlock_Logger.Text = exc.Message;
                }
                con.Close();
                button_Click_Search(null, null);
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sellect = comboBox.SelectedValue.ToString();
            setVisibilityComponents();
            if (sellect != "Alege Tip Adaugare")
            {
                add_button.Visibility = System.Windows.Visibility.Visible;
                SqlConnection con;
                string command;
                SqlCommand cmd;
                SqlDataReader rdr = null;

                switch (sellect)
                {
                    case "Adauga Deportat":
                        comboBox_Deportat_Adder.Items.Clear();
                        con = new SqlConnection(conn);
                        con.Open();
                        command = "select DENUMIRE from LOCALITATI";

                        cmd = new SqlCommand(command, con);

                        try
                        {
                            rdr = cmd.ExecuteReader();
                            textBlock_Logger.Text = "Succes";
                        }
                        catch (SqlException exc)
                        {
                            textBlock_Logger.Text = exc.Message;
                        }

                        comboBox_Deportat_Adder.FontFamily = new FontFamily("Courier New");

                        comboBox_Deportat_Adder.Items.Clear();
                        while (rdr.Read())
                        {
                            comboBox_Deportat_Adder.Items.Add(rdr.GetString(0));
                        }
                        rdr.Close();
                        con.Close();
                        comboBox_Deportat_Adder.Visibility = System.Windows.Visibility.Visible;
                        textBox_Deportat_Name.Visibility = System.Windows.Visibility.Visible;
                        textBox_Deportat_Prenume.Visibility = System.Windows.Visibility.Visible;
                        textBox_Deportat_Data_nasterii.Visibility = System.Windows.Visibility.Visible;
                        textBox_Deportat_Data_Decesului.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case "Adauga Descriere":
                        comboBox_Loc_Deportare_adder.Items.Clear();
                        comboBox_Loc_Reabilitare_adder.Items.Clear();

                        con = new SqlConnection(conn);
                        con.Open();
                        command = "select DENUMIRE from LOCALITATI";

                        cmd = new SqlCommand(command, con);

                        try
                        {
                            rdr = cmd.ExecuteReader();
                            textBlock_Logger.Text = "Succes";
                        }
                        catch (SqlException exc)
                        {
                            textBlock_Logger.Text = exc.Message;
                        }

                        comboBox_Loc_Reabilitare_adder.FontFamily = new FontFamily("Courier New");
                        comboBox_Loc_Deportare_adder.FontFamily = new FontFamily("Courier New");

                        comboBox_Loc_Deportare_adder.Items.Add("Loc Deportare");
                        comboBox_Loc_Reabilitare_adder.Items.Add("Loc Reabilitare");
                        comboBox_Loc_Deportare_adder.SelectedIndex = 0;
                        comboBox_Loc_Reabilitare_adder.SelectedIndex = 0;
                        while (rdr.Read())
                        {
                            comboBox_Loc_Deportare_adder.Items.Add(rdr.GetString(0));
                            comboBox_Loc_Reabilitare_adder.Items.Add(rdr.GetString(0));
                        }
                        rdr.Close();
                        con.Close();


                        comboBox_Loc_Deportare_adder.Visibility = System.Windows.Visibility.Visible;
                        comboBox_Loc_Reabilitare_adder.Visibility = System.Windows.Visibility.Visible;
                        textBox_Data_Deportare.Visibility = System.Windows.Visibility.Visible;
                        textBox_Data_Reabilitare.Visibility = System.Windows.Visibility.Visible;
                        break;

                    case "Adauga Localitate":
                        textBox_Localitate.Visibility = System.Windows.Visibility.Visible;
                        textBox_Localitate_Regiune.Visibility = System.Windows.Visibility.Visible;
                        break;

                    case "Adauga Regiune":
                        textBox_Regiune.Visibility = System.Windows.Visibility.Visible;
                        break;

                    case "Adauga Obiect":
                        textBox_Obiect.Visibility = System.Windows.Visibility.Visible;
                        textBox_Obiect_Utilizabil.Visibility = System.Windows.Visibility.Visible;
                        break;
                }
            }
        }

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            string sellect = comboBox.SelectedValue.ToString();
            setVisibilityComponents();
            if (sellect != "Alege Tip Adaugare")
            {
                add_button.Visibility = System.Windows.Visibility.Visible;
                SqlConnection con;
                string command;
                SqlCommand cmd;
                SqlDataReader rdr = null;

                switch (sellect)
                {
                    case "Adauga Deportat":
                        if (textBox_Deportat_Name.Text != "" && textBox_Deportat_Prenume.Text != "" && textBox_Deportat_Data_nasterii.Text != "" && comboBox_Deportat_Adder.SelectedIndex != -1)
                        {
                            con = new SqlConnection(conn);
                            con.Open();
                            command = "select ID_LOCALITATE from LOCALITATI where DENUMIRE=\'" + comboBox_Deportat_Adder.SelectedValue.ToString() + "\'";

                            cmd = new SqlCommand(command, con);

                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                            string result = "";
                            while (rdr.Read())
                            {
                                result = rdr.GetInt32(0).ToString();
                            }
                            rdr.Close();
                            if (result != "")
                            {
                                if (textBox_Deportat_Data_Decesului.Text == "")
                                    textBox_Deportat_Data_Decesului.Text = "NULL";
                                else
                                    textBox_Deportat_Data_Decesului.Text = "'" + textBox_Deportat_Data_Decesului.Text + "'";
                                command = "insert into DEPORTATI (NUME,PRENUME,DATA_NASTERII,DATA_DECES,ID_LOCALITATE_RESEDINTA) values ('" + textBox_Deportat_Name.Text + "','" + textBox_Deportat_Prenume.Text + "','" + textBox_Deportat_Data_nasterii.Text + "'," + textBox_Deportat_Data_Decesului.Text + ",'" + result + "')";
                                cmd = new SqlCommand(command, con);
                                try
                                {
                                    rdr = cmd.ExecuteReader();
                                    textBlock_Logger.Text = "Succes";
                                    rdr.Close();
                                }
                                catch (SqlException exc)
                                {
                                    textBlock_Logger.Text = exc.Message;
                                }
                            }
                            con.Close();
                            textBox_Deportat_Name.Text = "";
                            textBox_Deportat_Prenume.Text = "";
                            textBox_Deportat_Data_nasterii.Text = "";
                            textBox_Deportat_Data_Decesului.Text = "";

                            button_Click_Search(null, null);
                        }
                        break;
                    case "Adauga Descriere":
                        if ((state == 1 && textBox_Data_Deportare.Text != "" && comboBox_Loc_Deportare_adder.SelectedIndex != -1 && comboBox_Loc_Deportare_adder.SelectedIndex != 0 && listBox1.SelectedIndex != 0)
                            || (state == 2 && textBox_Data_Deportare.Text != "" && comboBox_Loc_Deportare_adder.SelectedIndex != -1 && comboBox_Loc_Deportare_adder.SelectedIndex != 0))//textBox_Deportat_Name.Text != "" && textBox_Deportat_Prenume.Text != "" && textBox_Deportat_Data_nasterii.Text != "" && comboBox_Loc_Deportare_adder.SelectedIndex != -1)
                        {
                            con = new SqlConnection(conn);
                            con.Open();
                            command = "select ID_LOCALITATE from LOCALITATI where DENUMIRE=\'" + comboBox_Loc_Deportare_adder.SelectedValue.ToString() + "\'";

                            cmd = new SqlCommand(command, con);

                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                            string loc_Deportare = "";
                            while (rdr.Read())
                            {
                                loc_Deportare = rdr.GetInt32(0).ToString();
                            }
                            rdr.Close();

                            string loc_Reabilitare = "NULL";
                            if (comboBox_Loc_Reabilitare_adder.SelectedIndex != -1 && comboBox_Loc_Reabilitare_adder.SelectedIndex != 0)
                            {
                                command = "select ID_LOCALITATE from LOCALITATI where DENUMIRE=\'" + comboBox_Loc_Reabilitare_adder.SelectedValue.ToString() + "\'";
                                cmd = new SqlCommand(command, con);

                                try
                                {
                                    rdr = cmd.ExecuteReader();
                                    textBlock_Logger.Text = "Succes";
                                }
                                catch (SqlException exc)
                                {
                                    textBlock_Logger.Text = exc.Message;
                                }
                                while (rdr.Read())
                                {
                                    loc_Reabilitare = rdr.GetInt32(0).ToString();
                                }
                                rdr.Close();
                            }
                            string id_Deportat = "";
                            if (state == 1)
                            {
                                id_Deportat = listBox1.SelectedItem.ToString();
                                id_Deportat = id_Deportat.Split(' ')[0];
                            }
                            else
                            {
                                id_Deportat = listBox1.Items.GetItemAt(0).ToString().Split(':')[1];
                            }
                            /* command = "select ID_LOCALITATE_DEPORTARE from DETALIIDEPORTARE where ID_DEPORTAT=" + id_Deportat;
                             cmd = new SqlCommand(command, con);
                             rdr = cmd.ExecuteReader();
                             string description_found = "";
                             while (rdr.Read())
                             {
                                 description_found = rdr.GetInt32(0).ToString();
                             }
                             rdr.Close();
                             if (description_found != "")
                             {*/
                            if (textBox_Data_Reabilitare.Text == "")
                                textBox_Data_Reabilitare.Text = "NULL";
                            else
                                textBox_Data_Reabilitare.Text = "'" + textBox_Data_Reabilitare.Text + "'";
                            if (loc_Reabilitare != "NULL")
                                loc_Reabilitare = "'" + loc_Reabilitare + "'";
                            command = "insert into DETALIIDEPORTARE (DATA_REABILITARE,ID_LOCALITATE_REABILITARE,DATA_DEPORTARE,ID_LOCALITATE_DEPORTARE,ID_DEPORTAT) values (" + textBox_Data_Reabilitare.Text + "," + loc_Reabilitare + ",'" + textBox_Data_Deportare.Text + "'," + loc_Deportare + ",'" + id_Deportat + "')";
                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                                rdr.Close();
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }


                            con.Close();
                            comboBox_Loc_Deportare_adder.SelectedIndex = 0;
                            comboBox_Loc_Reabilitare_adder.SelectedIndex = 0;
                            textBox_Data_Deportare.Text = "";
                            textBox_Data_Reabilitare.Text = "";

                            button_Click_Search(null, null);
                        }
                        break;

                    case "Adauga Localitate":

                        if (textBox_Localitate.Text != "" && textBox_Localitate_Regiune.Text != "")
                        {
                            con = new SqlConnection(conn);
                            con.Open();
                            command = "select ID from REPUBLICI where DENUMIRE='" + textBox_Localitate_Regiune.Text + "'";

                            cmd = new SqlCommand(command, con);

                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                            string id_region = "";
                            while (rdr.Read())
                            {
                                id_region = rdr.GetInt32(0).ToString();
                            }
                            if (id_region == "")
                                break;
                            rdr.Close();
                            command = "insert into LOCALITATI (DENUMIRE, ID_REPUBLICA) values('" + textBox_Localitate.Text + "','" + textBox_Localitate_Regiune.Text + "')";

                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }

                        }
                        break;
                    ////////////////////////////////////////////////////////////////////////
                    case "Adauga Regiune":
                        textBox_Regiune.Visibility = System.Windows.Visibility.Visible;
                        if (textBox_Regiune.Text != "")
                        {
                            con = new SqlConnection(conn);
                            con.Open();
                            command = "insert into REPUBLICI values('" + textBox_Regiune.Text + "')";

                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                    textBlock_Logger.Text = "Succes";
                                rdr.Close();
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                            textBox_Regiune.Text = "";
                        }
                        break;

                    case "Adauga Obiect":
                        textBox_Obiect.Visibility = System.Windows.Visibility.Visible;
                        textBox_Obiect_Utilizabil.Visibility = System.Windows.Visibility.Visible;

                        if (textBox_Obiect.Text != "" && textBox_Obiect_Utilizabil.Text != "")
                        {
                            con = new SqlConnection(conn);
                            con.Open();
                            command = "insert into OBIECTE values('" + textBox_Obiect.Text + "','" + textBox_Obiect_Utilizabil.Text + "')";

                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                    textBlock_Logger.Text = "Succes";
                                rdr.Close();
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                            textBox_Regiune.Text = "";
                        }
                        break;
                }
            }
            comboBox_SelectionChanged(null, null);
        }

        private void comboBox_adder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /* brief click event for viewing details about deportees
         * 
         */
        private void button_Details_Click(object sender, RoutedEventArgs e)
        {
            if (state == 1 && listBox1.SelectedIndex != -1 && listBox1.SelectedIndex != 0)
            {
                button_Details.Visibility = System.Windows.Visibility.Hidden;
                textBox_Update.Visibility = System.Windows.Visibility.Visible;
                button_update.Visibility = System.Windows.Visibility.Visible;
                state = 2;
                string selecteditem = "";
                if (selid == "")
                    selecteditem = listBox1.SelectedItem.ToString().Split(' ')[0];
                else
                    selecteditem = selid.ToString();
                SqlConnection con = new SqlConnection(conn);
                con.Open();

                SqlConnection con2 = new SqlConnection(conn);
                con2.Open();
                string command = "select * from Deportati where ID=" + selecteditem;

                SqlCommand cmd = new SqlCommand(command, con);

                SqlDataReader rdr = null;
                try
                {
                    rdr = cmd.ExecuteReader();
                    textBlock_Logger.Text = "Succes";
                }
                catch (SqlException exc)
                {
                    textBlock_Logger.Text = exc.Message;
                }
                SqlDataReader temp = null;

                string ID = "", ID_LOCALITATE_RESEDINTA = "NULL", NUME = "",
                    PRENUME = "", DATANASTERII = "", DATADECES = "", DATADEPORTARE = "",
                    DATAREABILITARE = "NULL", LOCALITATERESEDINTA = "",
                    ID_LOCALITATEREABILITARE = "NULL", ID_LOCALITATEDEPORTAT = "",
                    LOCALITATEREABILITARE = "NULL", LOCALITATEDEPORTAT = "",
                    OBIECTECONFISCATE = "";
                while (rdr.Read())
                {
                    ID = rdr.GetInt32(0).ToString();
                    NUME = rdr.GetString(1);
                    PRENUME = rdr.GetString(2);
                    DATANASTERII = rdr.GetDateTime(3).ToShortDateString();
                    if (!rdr.IsDBNull(4))
                    {
                        DATADECES = rdr.GetDateTime(4).ToShortDateString();
                    }
                    ID_LOCALITATE_RESEDINTA = rdr.GetInt32(5).ToString();
                }
                rdr.Close();

                command = "select * from DETALIIDEPORTARE where ID_DEPORTAT=" + selecteditem;

                cmd = new SqlCommand(command, con);

                try
                {
                    rdr = cmd.ExecuteReader();
                    textBlock_Logger.Text = "Succes";
                }
                catch (SqlException exc)
                {
                    textBlock_Logger.Text = exc.Message;
                }
                bool detaliiflag = false;//flag false if deportat doesnt have details
                while (rdr.Read())
                {
                    detaliiflag = true;
                    if (!rdr.IsDBNull(0))
                    {
                        DATAREABILITARE = rdr.GetDateTime(0).ToShortDateString(); ;
                    }
                    if (!rdr.IsDBNull(1))
                    {
                        ID_LOCALITATEREABILITARE = rdr.GetInt32(1).ToString();
                        command = "select DENUMIRE from LOCALITATI where ID_LOCALITATE=" + ID_LOCALITATEREABILITARE;

                        cmd = new SqlCommand(command, con2);
                        try
                        {
                            temp = cmd.ExecuteReader();
                            textBlock_Logger.Text = "Succes";
                        }
                        catch (SqlException exc)
                        {
                            textBlock_Logger.Text = exc.Message;
                        }
                        while (temp.Read())
                        {
                            LOCALITATEREABILITARE = temp.GetString(0);
                        }
                        temp.Close();
                    }
                    DATADEPORTARE = rdr.GetDateTime(2).ToShortDateString();
                    ID_LOCALITATEDEPORTAT = rdr.GetInt32(3).ToString();
                    command = "select DENUMIRE from LOCALITATI where ID_LOCALITATE=" + ID_LOCALITATEDEPORTAT;

                    cmd = new SqlCommand(command, con2);
                    try
                    {
                        rdr = cmd.ExecuteReader();
                        textBlock_Logger.Text = "Succes";
                    }
                    catch (SqlException exc)
                    {
                        textBlock_Logger.Text = exc.Message;
                    }
                    while (temp.Read())
                    {
                        LOCALITATEDEPORTAT = temp.GetString(0);
                    }
                    temp.Close();
                }
                rdr.Close();

                command = "select DENUMIRE from LOCALITATI where ID_LOCALITATE=" + ID_LOCALITATE_RESEDINTA;

                cmd = new SqlCommand(command, con);

                try
                {
                    rdr = cmd.ExecuteReader();
                    textBlock_Logger.Text = "Succes";
                }
                catch (SqlException exc)
                {
                    textBlock_Logger.Text = exc.Message;
                }
                while (rdr.Read())
                {
                    LOCALITATERESEDINTA = rdr.GetString(0);
                }
                rdr.Close();

                command = "select DENUMIRE from OBIECTE where ID IN (select ID_OBIECT from DetaliiConfiscari where ID_DEPORTAT=" + selecteditem + ")";
                string command2 = "select CANTITATE from DetaliiConfiscari where ID_DEPORTAT = " + selecteditem;
                cmd = new SqlCommand(command, con);
                SqlCommand cmd2 = new SqlCommand(command2, con2);
                if (temp != null)
                    temp.Close();
                try
                {
                    temp = cmd.ExecuteReader();
                    textBlock_Logger.Text = "Succes";
                }
                catch (SqlException exc)
                {
                    textBlock_Logger.Text = exc.Message;
                }
                try
                {
                    rdr = cmd.ExecuteReader();
                    textBlock_Logger.Text = "Succes";
                }
                catch (SqlException exc)
                {
                    textBlock_Logger.Text = exc.Message;
                }
                while (rdr.Read() && temp.Read())
                {
                    OBIECTECONFISCATE += temp.GetInt32(0) + " x " + rdr.GetString(0) + ", ";
                }
                if (OBIECTECONFISCATE.Length != 0)
                    OBIECTECONFISCATE = OBIECTECONFISCATE.Substring(0, OBIECTECONFISCATE.Length - 2);
                rdr.Close();
                listBox1.Items.Clear();
                listBox1.Items.Add("ID Deportat:" + ID);
                listBox1.Items.Add("NUME:" + NUME);
                listBox1.Items.Add("PRENUME:" + PRENUME);
                listBox1.Items.Add("DATA NASTERII:" + DATANASTERII);
                listBox1.Items.Add("DATA DECESULUI:" + DATADECES);
                listBox1.Items.Add("RESEDINTA PANA LA DEPORTARE:" + LOCALITATERESEDINTA);
                if (detaliiflag)
                {
                    listBox1.Items.Add("DATA DEPORTARE:" + DATADEPORTARE);
                    listBox1.Items.Add("LOCALITATE DEPORTARE:" + LOCALITATEDEPORTAT);
                    listBox1.Items.Add("DATA REABILITARE:" + DATAREABILITARE);
                    listBox1.Items.Add("LOCALITATE REABILITARE:" + LOCALITATEREABILITARE);
                }
                listBox1.Items.Add("OBIECTE CONFISCATE:" + OBIECTECONFISCATE);
            }
        }

        //Update entries from details view

        private void button_Update_Click(object sender, RoutedEventArgs e)
        {
            if (state == 2 && listBox1.SelectedIndex != -1 && textBox_Update.Text != "")
            {
                string command = "";
                string MAIN_ID = listBox1.Items.GetItemAt(0).ToString().Split(':')[1];
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd;// new SqlCommand(command, con);

                SqlDataReader rdr=null;//= cmd.ExecuteReader();

                string field = listBox1.SelectedItem.ToString().Split(':')[0];
                switch (field)
                {
                    case "NUME":
                        command = "update DEPORTATI SET NUME = '" + textBox_Update.Text + "' WHERE ID=" + MAIN_ID;
                        cmd = new SqlCommand(command, con);
                        try
                        {
                            rdr = cmd.ExecuteReader();
                            textBlock_Logger.Text = "Succes";
                        }
                        catch (SqlException exc)
                        {
                            textBlock_Logger.Text = exc.Message;
                        }
                        break;
                    case "PRENUME":
                        command = "update DEPORTATI SET PRENUME = '" + textBox_Update.Text + "' WHERE ID=" + MAIN_ID;
                        cmd = new SqlCommand(command, con);
                        try
                        {
                            rdr = cmd.ExecuteReader();
                            textBlock_Logger.Text = "Succes";
                        }
                        catch (SqlException exc)
                        {
                            textBlock_Logger.Text = exc.Message;
                        }
                        break;
                    case "DATA NASTERII":
                        command = "update DEPORTATI SET DATA_NASTERII = convert(date, '" + textBox_Update.Text + "',104)" + " WHERE ID=" + MAIN_ID;
                        cmd = new SqlCommand(command, con);
                        try
                        {
                            rdr = cmd.ExecuteReader();
                            textBlock_Logger.Text = "Succes";
                        }
                        catch (SqlException exc)
                        {
                            textBlock_Logger.Text = exc.Message;
                        }
                        break;
                    case "DATA DECESULUI":
                        if (textBox_Update.Text != "")
                        {
                            command = "update DEPORTATI SET DATA_DECES = convert(date, '" + textBox_Update.Text + "',104)" + " WHERE ID=" + MAIN_ID;
                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                        }
                        else
                        {
                            command = "update DEPORTATI SET DATA_DECES = NULL WHERE ID=" + MAIN_ID;
                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                        }

                        break;
                    case "RESEDINTA PANA LA DEPORTARE":
                        command = "update DEPORTATI SET ID_LOCALITATE_RESEDINTA = (select ID_LOCALITATE from LOCALITATI where lower(DENUMIRE)=lower('" + textBox_Update.Text + "')) WHERE ID=" + MAIN_ID;
                        cmd = new SqlCommand(command, con);
                        try
                        {
                            rdr = cmd.ExecuteReader();
                            textBlock_Logger.Text = "Succes";
                        }
                        catch (SqlException exc)
                        {
                            textBlock_Logger.Text = exc.Message;
                        }
                        break;
                    case "DATA DEPORTARE":
                        command = "update DETALIIDEPORTARE SET DATA_DEPORTARE = convert(date, '" + textBox_Update.Text + "',104) WHERE ID_DEPORTAT=" + MAIN_ID;
                        cmd = new SqlCommand(command, con);
                        try
                        {
                            rdr = cmd.ExecuteReader();
                            textBlock_Logger.Text = "Succes";
                        }
                        catch (SqlException exc)
                        {
                            textBlock_Logger.Text = exc.Message;
                        }
                        break;
                    case "LOCALITATE DEPORTARE":
                        command = "update DETALIIDEPORTARE SET ID_LOCALITATE_DEPORTARE = (select ID_LOCALITATE from LOCALITATI where lower(DENUMIRE)=lower('" + textBox_Update.Text + "')) WHERE ID_DEPORTAT=" + MAIN_ID;
                        cmd = new SqlCommand(command, con);
                        try
                        {
                            rdr = cmd.ExecuteReader();
                            textBlock_Logger.Text = "Succes";
                        }
                        catch (SqlException exc)
                        {
                            textBlock_Logger.Text = exc.Message;
                        }
                        break;
                    case "DATA REABILITARE":
                        if (textBox_Update.Text != "")
                        {
                            command = "update DETALIIDEPORTARE SET DATA_REABILITARE = convert(date, '" + textBox_Update.Text + "',104) WHERE ID_DEPORTAT=" + MAIN_ID;
                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                        }
                        else
                        {
                            command = "update DETALIIDEPORTARE SET DATA_REABILITARE = NULL WHERE ID_DEPORTAT=" + MAIN_ID;
                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                        }
                        break;
                    case "LOCALITATE REABILITARE":

                        if (textBox_Update.Text != "")
                        {
                            command = "update DETALIIDEPORTARE SET ID_LOCALITATE_REABILITARE = (select ID_LOCALITATE from LOCALITATI where lower(DENUMIRE)=lower('" + textBox_Update.Text + "')) WHERE ID_DEPORTAT=" + MAIN_ID;
                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                        }
                        else
                        {
                            command = "update DETALIIDEPORTARE SET ID_LOCALITATE_REABILITARE = NULL WHERE ID_DEPORTAT=" + MAIN_ID;
                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                        }
                        break;
                    case "OBIECTE CONFISCATE":
                        if (textBox_Update.Text != "")
                        {
                            string[] str = textBox_Update.Text.Split(new char[] { ' ' }, 2);

                            if (str[0] == "0")
                            {
                                command = "DELETE FROM DETALIICONFISCARI WHERE ID_OBIECT= (select ID from OBIECTE where lower(Denumire)=lower('" + str[1] + "'))";
                                cmd = new SqlCommand(command, con);
                                try
                                {
                                    rdr = cmd.ExecuteReader();
                                    textBlock_Logger.Text = "Succes";
                                }
                                catch (SqlException exc)
                                {
                                    textBlock_Logger.Text = exc.Message;
                                }
                            }
                            else
                            {
                                command = "delete from  DETALIICONFISCARI WHERE ID_OBIECT= (select ID from OBIECTE where lower(Denumire)=lower('" + str[1] + "'))";
                                cmd = new SqlCommand(command, con);
                                try
                                {
                                    rdr = cmd.ExecuteReader();
                                    textBlock_Logger.Text = "Succes";
                                }
                                catch (SqlException exc)
                                {
                                    textBlock_Logger.Text = exc.Message;
                                }
                                rdr.Close();
                                command = "insert into DETALIICONFISCARI values(" + MAIN_ID + ", (select ID from OBIECTE where lower(Denumire)=lower('" + str[1] + "')), " + str[0] + ")";
                                cmd = new SqlCommand(command, con);
                                try
                                {
                                    rdr = cmd.ExecuteReader();
                                    textBlock_Logger.Text = "Succes";
                                }
                                catch (SqlException exc)
                                {
                                    textBlock_Logger.Text = exc.Message;
                                }
                            }
                        }
                        else
                        {
                            command = "DELETE FROM DETALIICONFISCARI WHERE ID_DEPORTAT= " + MAIN_ID;
                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                        }
                        break;

                }
                selid = MAIN_ID;
                state = 1;
                button_Details_Click(null, null);
                selid = "";
            }
        }

        private void delete_entry_button_Click(object sender, RoutedEventArgs e)
        {
            string sellect = comboBox_toDelete.SelectedValue.ToString();
            setVisibilityComponents();
            if (sellect != "Alege Tip Adaugare")
            {
                SqlConnection con;
                string command;
                SqlCommand cmd;
                SqlDataReader rdr;

                switch (sellect)
                {
                    case "Sterge Localitate":

                        if (textBox_toDelete.Text != "")
                        {
                            con = new SqlConnection(conn);
                            con.Open();
                            command = "delete from LOCALITATI where DENUMIRE='" + textBox_toDelete.Text + "'";

                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                        }
                        break;
                    ////////////////////////////////////////////////////////////////////////
                    case "Sterge Regiune":

                        if (textBox_toDelete.Text != "")
                        {
                            con = new SqlConnection(conn);
                            con.Open();
                            command = "delete from REPUBLICI where DENUMIRE='" + textBox_toDelete.Text + "'";

                            cmd = new SqlCommand(command, con);
                            try
                            {
                                rdr = cmd.ExecuteReader();
                                textBlock_Logger.Text = "Succes";
                            }
                            catch (SqlException exc)
                            {
                                textBlock_Logger.Text = exc.Message;
                            }
                        }
                        break;

                    case "Sterge Obiect":
                        con = new SqlConnection(conn);
                        con.Open();
                        command = "delete from OBIECTE where DENUMIRE='" + textBox_toDelete.Text + "'";

                        cmd = new SqlCommand(command, con);
                        try
                        {
                            rdr = cmd.ExecuteReader();
                            textBlock_Logger.Text = "Succes";
                        }
                        catch (SqlException exc)
                        {
                            textBlock_Logger.Text = exc.Message;
                        }
                        break;
                }
            }
            comboBox_SelectionChanged(null, null);

        }
    }
}
