using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.GroupRowLayout;
using RibbonApplicationOpgi.Model;
using MessageBox = System.Windows.Forms.MessageBox;


namespace RibbonApplicationOpgi.Windows
{
    /// <summary>
    /// Interaction logic for ProgrammeWind.xaml
    /// </summary>
    public partial class ProgrammeWind : ThemedWindow
    {
        private FiltreEntities FiltreEntities = new FiltreEntities();
        private string operation = "";
        public ProgrammeWind()
        {
            InitializeComponent();

            ChargementGridControl();
            ChargementLayoutControl();
        }
        
        private void ChargementGridControl()
        {
            try
            {
                var query = from q in FiltreEntities.Programmes select q;
                if (query.Any())
                    GridControl.ItemsSource = GetDataProgramme(query);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.InnerException, "Erreur de chergement des Programmes",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private DataTable GetDataProgramme(IQueryable<Programme> query)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Designation", typeof(string));

            foreach (var VARIABLE in query)
            {
                var row = dataTable.NewRow();
                row["Id"] = VARIABLE.Id;
                row["Designation"] = VARIABLE.DesignationProgramme;

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private void ChargementLayoutControl()
        {
            Programme programme = FindProgramme();
            if (programme != null)
            {
                IdItem.Text = programme.Id.ToString();
                DesignationItem.Text = programme.DesignationProgramme;
            }
            else
            {
                IdItem.Clear();
                DesignationItem.Clear();
            }
            
        }

        private Programme FindProgramme()
        {
            try
            {
                Programme programme = new Programme();
                var selRow = TableView.FocusedRowHandle;
                if (selRow >= 0)
                {
                    int id = Convert.ToInt32(GridControl.GetFocusedRowCellValue("Id").ToString());
                    //int id = 1;
                    var query = from q in FiltreEntities.Programmes
                        where q.Id == id
                        select q;
                    if (query.Any())
                        programme = query.FirstOrDefault();
                }

                return programme;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.InnerException, "Erreur de retrouver le Programme",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void TableView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            ChargementLayoutControl();
        }

        private void Deletebtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Programme programme = new Programme();

                DialogResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer ce programme?", "Confirmation!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (messageBoxResult == System.Windows.Forms.DialogResult.Yes)
                {

                    var selRow = TableView.FocusedRowHandle;
                    if (selRow > 0)
                    {
                        int id = Convert.ToInt32(GridControl.GetFocusedRowCellValue("Id").ToString());
                        var query = from q in FiltreEntities.Programmes
                            where q.Id == id
                            select q;
                        if (query.Any())
                        {
                            programme = query.FirstOrDefault();
                            FiltreEntities.Programmes.Remove(programme);
                            FiltreEntities.SaveChanges();

                            TableView.FocusedRowHandle = selRow - 1;
                            ChargementGridControl();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.InnerException, "Erreur de Suppression du Programme",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Addbtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ModeAjout();
            operation = "Ajout";
        }

        private void Updatebtn_Click(object sender, RoutedEventArgs e)
        {
            ModeEdition();
            operation = "Edition";
        }

        private void Validatebtn_Click(object sender, RoutedEventArgs e)
        {
            if (operation == "Ajout")
                AddProgramme();
            if (operation == "Edition")
                EditProgramme();

            ModeNormale();
        }
        
        private void Cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            ModeNormale();
        }

        private void ModeAjout()
        {
            Validatebtn.Visibility = Visibility.Visible;
            Cancelbtn.Visibility = Visibility.Visible;
            Addbtn.IsEnabled = false;
            Updatebtn.IsEnabled = false;
            Deletebtn.IsEnabled = false;

            GridControl.IsEnabled = false;

            IdItem.Clear();
            DesignationItem.Clear();
        }

        private void ModeEdition()
        {
            Validatebtn.Visibility = Visibility.Visible;
            Cancelbtn.Visibility = Visibility.Visible;
            Addbtn.IsEnabled = false;
            Updatebtn.IsEnabled = false;
            Deletebtn.IsEnabled = false;

            GridControl.IsEnabled = false;
        }

        private void ModeNormale()
        {
            Validatebtn.Visibility = Visibility.Hidden;
            Cancelbtn.Visibility = Visibility.Hidden;
            Addbtn.IsEnabled = true;
            Updatebtn.IsEnabled = true;
            Deletebtn.IsEnabled = true;

            GridControl.IsEnabled = true;

            ChargementLayoutControl();
        }

        private void AddProgramme()
        {
            try
            {
                if (DesignationItem.Text != String.Empty)
                {
                    if (ProgrammeExist(DesignationItem.Text))
                    {
                        MessageBox.Show("Ce programme existe déjà!", "Erreur!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Programme programme = new Programme();
                    programme.DesignationProgramme = DesignationItem.Text;

                    FiltreEntities.Programmes.Add(programme);
                    FiltreEntities.SaveChanges();

                    ChargementGridControl();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.InnerException, "Erreur d'ajout du Programme",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditProgramme()
        {
            try
            {
                int id = Int32.Parse(IdItem.Text);
                var query = from q in FiltreEntities.Programmes
                    where q.Id == id
                    select q;
                if (query.Any())
                {
                    Programme programme = new Programme();
                    programme = query.FirstOrDefault();

                    if (ProgrammeExist(DesignationItem.Text) && GetId(DesignationItem.Text) != programme.Id)
                    {
                        MessageBox.Show("Ce programme existe déjà!", "Erreur!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    programme.DesignationProgramme = DesignationItem.Text;

                    FiltreEntities.SaveChanges();

                    ChargementGridControl();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.InnerException, "Erreur d'edition du Programme",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetId(string designationItemText)
        {
            var query = from q in FiltreEntities.Programmes
                where q.DesignationProgramme == designationItemText
                select q;
            return query.FirstOrDefault().Id;

            /*var query2 = from q in FiltreEntities.Programmes
                from q2 in FiltreEntities.Postulants
                from q3 in FiltreEntities.Projets 
                         where q.DesignationProgramme == "LPA"
                         where q2.Date_Demande >= DateTime.Parse("01/01/2020")
                         where q3.Commune == "Corso"
                    select q2;*/
        }

        private bool ProgrammeExist(string designationItemText)
        {
            var query = from q in FiltreEntities.Programmes 
                where q.DesignationProgramme == designationItemText
                        select q;
            return query.Any();
        }

        private void Exportbtn_Click(object sender, RoutedEventArgs e)
        {
            /*SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Document Excel (*.xlsx)|*.xlsx";
            dialog.DefaultExt = ".xlsx";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    GridControl.View.ExportToXlsx(dialog.FileName);
                    MessageBox.Show("Exportation effectuée avec succès ", "Succès!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Exportation impossible: " + exp.Message, "Erreur!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                dialog.Dispose();
            }*/

            //GridControl.View.ShowPrintPreview(this,"Nom du document Test", "Titre Test");
            GridControl.View.Print();
        }
    }
}
