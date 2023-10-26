using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Assistant
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        internal ComboBox ProfilesComboBox { get { return profilesComboBox; } }

        private void profilesAddButton_Click(object sender, EventArgs e)
        {
            EnhancedProfileAdd addprofile = new EnhancedProfileAdd
            {
                TopMost = true
            };
            addprofile.Show();
        }

        private void profilesDeleteButton_Click(object sender, EventArgs e)
        {
            string profiletodelete = profilesComboBox.Text;

            DialogResult dialogResult = MessageBox.Show("Are you sure to delete this profile: " + profiletodelete, "Delete Profile?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (profiletodelete == "default")
                {
                    MessageBox.Show("Can't delete default profile",
                    "Can't delete default profile!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                }
                else
                {
                    RazorEnhanced.Profiles.SetLast("default");
                    RazorEnhanced.Profiles.Delete(profiletodelete);
                    RazorEnhanced.Profiles.Refresh();
                    RazorEnhanced.Profiles.ProfileChange("default");
                    try
                    {
                        string filename = Path.Combine(Assistant.Engine.RootPath, "Profiles", profiletodelete);
                        var dir = new DirectoryInfo(filename);
                        dir.Delete(true);
                    }
                    catch
                    { }
                }
            }
        }

        // must be run in ui thread
        internal void changeProfile(string profileName)
        {
            int newIndex = Assistant.Engine.MainWindow.ProfilesComboBox.FindStringExact(profileName);
            if (newIndex == -1)
            {
                Misc.SendMessage(String.Format("Unable to find profile {0}", profileName), 54, true);
                return;
            }

            if (profileName != Assistant.Engine.MainWindow.ProfilesComboBox.SelectedValue)
            {
                RazorEnhanced.Profiles.ProfileChange(profileName);
                Engine.MainWindow.ProfilesComboBox.SelectedIndex = Engine.MainWindow.ProfilesComboBox.FindString(profileName);
            }
        }

        private void profilesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (profilesComboBox.Focused)
            {
                if (RazorEnhanced.Profiles.LastUsed() != profilesComboBox.Text)
                {
                    RazorEnhanced.Profiles.ProfileChange(profilesComboBox.Text);
                }
            }
            var linkName = RazorEnhanced.Profiles.GetLinkName(profilesComboBox.Text);
            m_Tip.SetToolTip(profilelinklabel, linkName);
            if (linkName.Length > 25)
            {
                linkName = linkName.Substring(0, 25);
                linkName += "...";
            }
            profilelinklabel.Text = "Linked to: " + linkName;
        }

        private void profilesLinkButton_Click(object sender, EventArgs e)
        {
            if (World.Player == null)
            {
                MessageBox.Show("You can't link a profile to player if not logged in game",
                "Profiles",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                return;
            }

            RazorEnhanced.Profiles.Link(World.Player.Serial, profilesComboBox.Text, World.Player.Name);
            var linkName = RazorEnhanced.Profiles.GetLinkName(profilesComboBox.Text);
            m_Tip.SetToolTip(profilelinklabel, linkName);
            if (linkName.Length > 25)
            {
                linkName = linkName.Substring(0, 25);
                linkName += "...";
            }
            profilelinklabel.Text = "Linked to: " + linkName;
            Misc.SendMessage("Profile: " + profilesComboBox.Text + " linked to player: " + RazorEnhanced.Profiles.GetLinkName(profilesComboBox.Text), false);
        }

        private void profilesUnlinkButton_Click(object sender, EventArgs e)
        {
            RazorEnhanced.Profiles.UnLink(profilesComboBox.Text, World.Player.Serial);
            var linkName = RazorEnhanced.Profiles.GetLinkName(profilesComboBox.Text);
            m_Tip.SetToolTip(profilelinklabel, linkName);
            if (linkName.Length > 25)
            {
                linkName = linkName.Substring(0, 25);
                linkName += "...";
            }
            profilelinklabel.Text = "Linked to: " + linkName;
        }

        private void profilesRenameButton_Click(object sender, EventArgs e)
        {
            if (profilesComboBox.Text == "default")
            {
                MessageBox.Show("Can't rename default profile",
                "Can't rename default profile!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
            }
            else
            {
                EnhancedProfileRename renameprofile = new EnhancedProfileRename
                {
                    TopMost = true
                };
                renameprofile.Show();
            }
        }

        private void profilesCloneButton_Click(object sender, EventArgs e)
        {
            EnhancedProfileClone cloneprofile = new EnhancedProfileClone
            {
                TopMost = true
            };
            cloneprofile.Show();
        }
    }
}
