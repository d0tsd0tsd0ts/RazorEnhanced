﻿using System;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedRestockEditItem : Form
	{
		private const string m_Title = "Enhanced Restcok Edit Item";

		private string m_List;
		private Restock.RestockItem m_Item;
		private int m_Index;

		public EnhancedRestockEditItem(string list, int index, Restock.RestockItem item)
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title;

			m_List = list;
			m_Index = index;
			m_Item = item;
		}

		private void EnhancedRestockManualAdd_Load(object sender, EventArgs e)
		{
			tName.Text = m_Item.Name;
			tGraphics.Text = "0x" + m_Item.Graphics.ToString("X4");
			if (m_Item.Color == -1)
				tColor.Text = "-1";
			else
				tColor.Text = "0x" + m_Item.Color.ToString("X4");
			tAmount.Text = m_Item.AmountLimit.ToString();
		}

		private void bClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void bAddItem_Click(object sender, EventArgs e)
		{
			bool fail = false;
			int graphics = 0;
			int color = 0;
			int amount = 0;
			if (tName.Text == null)
			{
				MessageBox.Show("Item name is not valid.",
				"Item name Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}

			try
			{
				graphics = Convert.ToInt32(tGraphics.Text, 16);
			}
			catch
			{
				MessageBox.Show("Item Graphics is not valid.",
				"Item Graphics Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}

			try
			{
				amount = Convert.ToInt32(tAmount.Text);
			}
			catch
			{
				MessageBox.Show("Item Amount is not valid.",
				"Item Amount Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}

			if (tColor.Text == "-1")
				color = -1;
			else
			{
				try
				{
					color = Convert.ToInt32(tColor.Text, 16);
				}
				catch
				{
					MessageBox.Show("Item Color is not valid.",
					"Item Color Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation,
					MessageBoxDefaultButton.Button1);
					fail = true;
				}
			}
			if (!fail)
			{
				RazorEnhanced.Restock.ModifyItemInList(tName.Text, graphics, color, amount, m_Item.Selected, m_Item, m_Index);
				RazorEnhanced.Restock.RefreshItems();
				this.Close();
			}
		}
	}
}