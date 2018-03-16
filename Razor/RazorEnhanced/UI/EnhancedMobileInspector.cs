﻿using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using Assistant;

namespace RazorEnhanced.UI
{
	internal partial class EnhancedMobileInspector : Form
	{
		private Thread m_ProcessInfo;
		private Assistant.Mobile m_mobile;

		internal EnhancedMobileInspector(Assistant.Mobile mobileTarg)
		{
			InitializeComponent();
			m_mobile = mobileTarg;
			MaximizeBox = false;
		}

		private void ProcessInfoThread()
		{
			foreach (string prop in m_props)
			{
				int attrib = 0;

				attrib = GetAttribute(prop);
				if (attrib > 0)
					AddAttributesToList(Assistant.Utility.CapitalizeAllWords(prop) + ": "+ attrib);
			}
		}

		private void AddAttributesToList(string value)
		{
			try
			{
				if (Assistant.Engine.Running)
				{
					listBoxAttributes.Invoke(new Action(() => listBoxAttributes.Items.Add(value)));
				}
			}
			catch { }
		}

		// Props to show
		private List<string> m_props = new List<string>
		{
			"Fire Resist",
			"Cold Resist",
			"Poison Resist",
			"Energy Resist",
			"Physical Resist",
			"Swing Speed Increase",
			"Damage Chance Increase",
			"Damage Increase",
			"Hit Fireball",
			"Hit Chance Increase",
			"Mage Armor",
			"Lower Reagent Cost",
			"Hit Point Increase",
			"Hit Point Regeneration",
			"Stamina Regeneration",
			"Mana Regeneration",
			"Reflect Physical Damage",
			"Enhance Potions",
			"Defense Chance Increase",
			"Spell Damage Increase",
			"Faster Cast Recovery",
			"Faster Casting",
			"Lower Mana Cost",
			"Strength Increase",
			"Dexterity Increase",
			"Dexterity Bonus",
			"Intelligence Bonus",
			"Strength Bonus",
			"Intelligence Increase",
			"Hit Point Increase",
			"Stamina Increase",
			"Mana Increase",
			"Maximum Hit Point Increase",
			"Maximum Stamina Increase",
			"Maximum Mana Increase",
			"Self Repair",
			"Luck",
			"Hit Lower Defense",
			// Sa props
			"Casting Focus",
			"Fire Eater",
			"Energy Eaters",
			"Cold Eaters",
			"Poison Eater",
			"Damage Eater",
			"Kinetic Eater",
		};

		// Layer to scan
		private static List<Assistant.Layer> m_layer_props = new List<Layer>
		{
			Layer.RightHand,
			Layer.LeftHand,
			Layer.Shoes,
			Layer.Pants,
			Layer.Shirt,
			Layer.Head,
			Layer.Gloves,
			Layer.Ring,
			Layer.Talisman,
			Layer.Neck,
			Layer.Waist,
			Layer.InnerTorso,
			Layer.Bracelet,
			Layer.Unused_xF,
			Layer.MiddleTorso,
			Layer.Earrings,
			Layer.Arms,
			Layer.Cloak,
			Layer.Backpack,
			Layer.OuterTorso,
			Layer.OuterLegs,
			Layer.InnerLegs
		};

		private int GetAttribute(string attributename)
		{
			int attributevalue = 0;

			foreach (Layer l in m_layer_props)
			{
				Assistant.Item itemtocheck = m_mobile.GetItemOnLayer(l);
				if (itemtocheck == null) // Slot vuoto
					continue;

				if (!itemtocheck.PropsUpdated)
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);

				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}
			return attributevalue;
        }

		private void razorButton1_Click(object sender, EventArgs e)
		{
			try
			{
				m_ProcessInfo.Abort();
			}
			catch { }

			this.Close();
		}

		private void bNameCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lName.Text);
		}

		private void bSerialCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lSerial.Text);
		}

		private void bItemIdCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lMobileID.Text);
		}

		private void bColorCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lColor.Text);
		}

		private void bPositionCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lPosition.Text);
		}

		private void bContainerCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lSex.Text);
		}

		private void bRContainerCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lHits.Text);
		}

		private void bAmountCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lMaxHits.Text);
		}

		private void bLayerCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lNotoriety.Text);
		}

		private void bOwnedCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lDirection.Text);
		}

		private void EnhancedMobileInspector_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				m_ProcessInfo.Abort();
			}
			catch { }
		}



		private void EnhancedMobileInspector_Load(object sender, EventArgs e)
		{
			if (m_mobile == null)
				Close();

			// general
			lName.Text = m_mobile.Name.ToString();
			lSerial.Text = "0x" + m_mobile.Serial.Value.ToString("X8");
			lMobileID.Text = "0x" + m_mobile.Body.ToString("X4");
			lColor.Text = "0x" + m_mobile.Hue.ToString("X4");
			lPosition.Text = m_mobile.Position.ToString();

			// Details
			lSex.Text = (m_mobile.Female) ? "Female" : "Male";

			lHits.Text = m_mobile.Hits.ToString();
			lMaxHits.Text = m_mobile.Hits.ToString();
			lNotoriety.Text = m_mobile.Notoriety.ToString();

			switch (m_mobile.Direction & Assistant.Direction.Mask)
			{
				case Assistant.Direction.North: lDirection.Text = "North"; break;
				case Assistant.Direction.South: lDirection.Text = "South"; break;
				case Assistant.Direction.West: lDirection.Text = "West"; break;
				case Assistant.Direction.East: lDirection.Text = "East"; break;
				case Assistant.Direction.Right: lDirection.Text = "Right"; break;
				case Assistant.Direction.Left: lDirection.Text = "Left"; break;
				case Assistant.Direction.Down: lDirection.Text = "Down"; break;
				case Assistant.Direction.Up: lDirection.Text = "Up"; break;
				default: lDirection.Text = "Undefined"; break;
			}

			// Flag
			lFlagPoisoned.Text = (m_mobile.Poisoned) ? "Yes" : "No";
			lFlagPoisoned.ForeColor = (m_mobile.Poisoned) ? Color.Green : Color.Red;

			lFlagWar.Text = (m_mobile.Warmode) ? "Yes" : "No";
			lFlagWar.ForeColor = (m_mobile.Warmode) ? Color.Green : Color.Red;

			lFlagHidden.Text = (m_mobile.Visible) ? "No" : "Yes";
			lFlagHidden.ForeColor = (m_mobile.Visible) ? Color.Red : Color.Green;

			lFlagGhost.Text = (m_mobile.IsGhost) ? "Yes" : "No";
			lFlagGhost.ForeColor = (m_mobile.IsGhost) ? Color.Green : Color.Red;

			lFlagBlessed.Text = (m_mobile.Blessed) ? "Yes" : "No";
			lFlagBlessed.ForeColor = (m_mobile.Blessed) ? Color.Green : Color.Red;

			lFlagParalized.Text = (m_mobile.Paralized) ? "Yes" : "No";
			lFlagParalized.ForeColor = (m_mobile.Paralized) ? Color.Green : Color.Red;

			lFlagFlying.Text = (m_mobile.Flying) ? "Yes" : "No";
			lFlagFlying.ForeColor = (m_mobile.Flying) ? Color.Green : Color.Red;

			// Immagine
			try
			{
				int m_hue = m_mobile.Hue;
				Ultima.Frame[] m_animationframe = Ultima.Animations.GetAnimation(m_mobile.Body, 0, 1, ref m_hue, false, true);
				imagepanel.BackgroundImage = m_animationframe[0].Bitmap;
			}
			catch
			{ }

			for (int i = 0; i < m_mobile.ObjPropList.Content.Count; i++)
			{
				Assistant.ObjectPropertyList.OPLEntry ent = m_mobile.ObjPropList.Content[i];
				if (i == 0)
					lName.Text = ent.ToString();
				else
				{
					string content = ent.ToString();
					listBoxAttributes.Items.Add(content);
				}
			}

			if (m_mobile.ObjPropList.Content.Count == 0)
			{
				lName.Text = m_mobile.Name.ToString();
			}


			if (m_mobile == Assistant.World.Player)
			{
				listBoxAttributes.Items.Add(String.Empty);
				listBoxAttributes.Items.Add("Weight: " + Assistant.World.Player.Weight);
				if (Assistant.World.Player.Expansion >= 3)
				{
					listBoxAttributes.Items.Add("Stat Cap: " + Assistant.World.Player.StatCap);
					listBoxAttributes.Items.Add("Followers: " + Assistant.World.Player.Followers);
					listBoxAttributes.Items.Add("Max Followers: " + Assistant.World.Player.FollowersMax);

					if (Assistant.World.Player.Expansion >= 4)
					{
						listBoxAttributes.Items.Add("Damage Minimum: " + Assistant.World.Player.DamageMin);
						listBoxAttributes.Items.Add("Damage Maximum: " + Assistant.World.Player.DamageMax);
						listBoxAttributes.Items.Add("Tithing points: " + Assistant.World.Player.Tithe);

						if (Assistant.World.Player.Expansion >= 5)
						{
							switch (Assistant.World.Player.Race)
							{
								case 1:
									listBoxAttributes.Items.Add("Race: Human");
									break;
								case 2:
									listBoxAttributes.Items.Add("Race: Elf");
									break;
								case 3:
									listBoxAttributes.Items.Add("Race: Gargoyle");
									break;
							}

							listBoxAttributes.Items.Add("Max Weight: " + Assistant.World.Player.MaxWeight);

							if (Assistant.World.Player.Expansion >= 6)
							{
								m_ProcessInfo = new Thread(ProcessInfoThread);
								m_ProcessInfo.Start();
							}
						}
					}
				}
			}
			else
			{
				m_ProcessInfo = new Thread(ProcessInfoThread);
				m_ProcessInfo.Start();
			}
		}
	}
}