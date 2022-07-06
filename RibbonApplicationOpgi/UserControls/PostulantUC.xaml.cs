using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Grid;
using RibbonApplicationOpgi.Model;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;


namespace RibbonApplicationOpgi.UserControls
{
    /// <summary>
    /// Interaction logic for PostulantUC.xaml
    /// </summary>
    public partial class PostulantUC : UserControl
    {
        private FiltreEntities FiltreEntities = new FiltreEntities();
        private string operation = "";
        public PostulantUC()
        {
            InitializeComponent();
            ChargementGridControl();
            ChargementLayoutControl();
        }

        private void ChargementGridControl()
        {
            try
            {
                var query = from q in FiltreEntities.Postulants select q;
                if (query.Any())
                    GridControl.ItemsSource = GetDataPostulant(query);
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message + "\n" + exp.InnerException, "Erreur de chargement des Postulants",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private DataTable GetDataPostulant(IQueryable<Postulant> query)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Reference", typeof(string));
            dataTable.Columns.Add("Valide", typeof(string));
            dataTable.Columns.Add("Date Demande", typeof(string));
            dataTable.Columns.Add("Programme", typeof(string));
            dataTable.Columns.Add("Nom", typeof(string));
            dataTable.Columns.Add("Nom Jeune", typeof(string));
            dataTable.Columns.Add("Prenom", typeof(string));
            dataTable.Columns.Add("Sexe", typeof(string));
            



            foreach (var VARIABLE in query)
            {
                var row = dataTable.NewRow();
                row["Id"] = VARIABLE.Id;
                row["Reference"] = VARIABLE.Ref;
                row["Valide"] = VARIABLE.Valide;
                row["Date Demande"] = VARIABLE.Date_Demande;
                row["Programme"] = VARIABLE.Id_programme;
                row["Nom"] = VARIABLE.nom;
                row["Nom Jeune"] = VARIABLE.Nom_jeune;
                row["Prenom"] = VARIABLE.Prenom;
                row["Sexe"] = VARIABLE.Sexe;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private void ChargementLayoutControl()
        {
            Postulant postulant = FindPostulant();
            if (postulant != null)
            {
                ReferenceItem.Text = postulant.Ref;
                
            }
            else
            {

                ReferenceItem.Clear();
            }

        }

        private Postulant FindPostulant()
        {
            try
            {
                Postulant  postulant = new Postulant();
                var selRow = TableView.FocusedRowHandle;
                if (selRow >= 0)
                {
                    int id = Convert.ToInt32(GridControl.GetFocusedRowCellValue("Id").ToString());
                    //int id = 1;
                    var query = from q in FiltreEntities.Postulants
                        where q.Id == id
                        select q;
                    if (query.Any())
                        postulant = query.FirstOrDefault();
                }

                return postulant;
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message + "\n" + exp.InnerException, "Erreur de retrouver le postulant",
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
                Postulant postulant = new Postulant();

                DialogResult messageBoxResult = System.Windows.Forms.MessageBox.Show("Voulez-vous vraiment supprimer ce postulant?", "Confirmation!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (messageBoxResult == System.Windows.Forms.DialogResult.Yes)
                {

                    var selRow = TableView.FocusedRowHandle;
                    if (selRow > 0)
                    {
                        int id = Convert.ToInt32(GridControl.GetFocusedRowCellValue("Id").ToString());
                        var query = from q in FiltreEntities.Postulants
                            where q.Id == id
                            select q;
                        if (query.Any())
                        {
                            postulant = query.FirstOrDefault();
                            FiltreEntities.Postulants.Remove(postulant);
                            FiltreEntities.SaveChanges();

                            TableView.FocusedRowHandle = selRow - 1;
                            ChargementGridControl();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message + "\n" + exp.InnerException, "Erreur de Suppression du Postulant",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ModeAjout()
        {
            Validatebtn.Visibility = Visibility.Visible;
            Cancelbtn.Visibility = Visibility.Visible;
            Addbtn.IsEnabled = false;
            Updatebtn.IsEnabled = false;
            Deletebtn.IsEnabled = false;

            GridControl.IsEnabled = false;

            
            ReferenceItem.Clear();
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

        private void Addbtn_Click(object sender, RoutedEventArgs e)

        {
            ModeAjout();
            operation = "Ajout";
        }

        private void Validatebtn_Click(object sender, RoutedEventArgs e)
        {
            if (operation == "Ajout")
                AddPostulant();
            

            ModeNormale();
        }

       /* private void AddPostulant()
        {
            try
            {
                if (ReferenceItem.Text != String.Empty)
                {
                    if (PostulantExist(ReferenceItem.Text))
                    {
                        System.Windows.Forms.MessageBox.Show("Ce postulant existe déjà!", "Erreur!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Programme programme = new Programme();
                    programme.DesignationProgramme = ReferenceItem.Text;

                    FiltreEntities.Postulants.Add(programme);
                    FiltreEntities.SaveChanges();

                    ChargementGridControl();
                }
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message + "\n" + exp.InnerException, "Erreur d'ajout du Postulant",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
    }
}
