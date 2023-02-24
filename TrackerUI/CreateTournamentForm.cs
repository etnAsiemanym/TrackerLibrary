using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All();
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes= new List<PrizeModel>();
        
        public CreateTournamentForm()
        {
            InitializeComponent();

            InitializeLists();
        }

        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;

            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null;

            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;

            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void InitializeLists()
        {
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember= "TeamName";

            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void teamOneScoreLabel_Click(object sender, EventArgs e)
        {

        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem;

            if (t != null)
            {
                availableTeams.Remove(t);
                selectedTeams.Add(t);

                WireUpLists();
            }
            
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            // Call the CreatePrizeForm
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();
            // Get back a PrizeModel from the form
            // Take the PrizeModel and put it into our list of selected prizes
        }

        public void PrizeComplete(PrizeModel model)
        {
            selectedPrizes.Add(model);
            WireUpLists();
        }



        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            WireUpLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        private void removeSelectedPlayersButton_Click(object sender, EventArgs e)
        {
            //PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;

            //if (p != null)
            //{
            //    selectedTeamMembers.Remove(p);
            //    availableTeamMembers.Add(p);

            //    WireUpLists();
            //}

            TeamModel t = (TeamModel)tournamentTeamsListBox.SelectedItem;
            
            if (t != null) 
            { 
                selectedTeams.Remove(t);
                availableTeams.Add(t);
                
                WireUpLists();
            }
        }
        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel p = (PrizeModel)prizesListBox.SelectedItem;

            if (p != null)
            {
                selectedPrizes.Remove(p);

                WireUpLists();
            }
        }
        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            // Validate data
            decimal fee = 0;

            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text, out fee);
            
            if (!feeAcceptable) 
            {
                MessageBox.Show("You need to enter a valid entry fee.", 
                    "Invalid fee", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

 
            TournamentModel tm = new TournamentModel();

            tm.TournamentName = tournamentNameValue.Text;
            tm.EntryFee = fee;
            
            tm.Prizes = selectedPrizes;
            tm.EnteredTeams = selectedTeams;

            // TODO - Wire up the matchups
            // Order our list randomly - no bias who matches up against whom
            // Check if it's big enough - if not, add in byes
            // Create our first round of matchups
            // Create every round after that

            // Create Tournament entry 
            // Create all of the team entries
            // Create all of the prizes entries

            GlobalConfig.Connection.CreateTournament(tm);

            
        }

        
    }
}