using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvancedTextEditor
{
    public partial class AdvancedTextEditor : Form
    {
        private int TabCount = 0;

        public AdvancedTextEditor()
        {
            InitializeComponent();
            AddTab();                   //Opens the page with a blank tab TODO: be able to open documents directly into the program
            DrawComboBox1Items();
            PopulateFontSizes();
            int Count = 0;
            foreach (FontFamily FFam in toolStripComboBox1.Items)
            {
                if (FFam.Name == "Segoe UI")
                {
                    toolStripComboBox1.SelectedIndex = Count;
                    Count++;
                }
            }
        }

        #region Methods

        #region Tabs

        private void AddTab()
        {
            RichTextBox Body = new RichTextBox();

            Body.Name = "Body";
            Body.Dock = DockStyle.Fill;
            Body.ContextMenuStrip = contextMenuStrip1;
            Body.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            Body.ForeColor = System.Drawing.SystemColors.ControlLight;

            TabPage NewPage = new TabPage();
            TabCount += 1;

            string DocumentText = "Document " + TabCount;
            NewPage.Name = DocumentText;
            NewPage.Text = DocumentText;
            NewPage.Controls.Add(Body);
            NewPage.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            NewPage.ForeColor = System.Drawing.SystemColors.ControlLight;

            tabControl1.TabPages.Add(NewPage);
        }

        private void RemoveTab()
        {
            if (tabControl1.TabPages.Count != 1)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                AddTab();
            }
        }

        private void RemoveAllTabs()
        {
            foreach (TabPage Page in tabControl1.TabPages)
            {
                tabControl1.TabPages.Remove(Page);
            }

            AddTab();
        }

        private void RemoveAllTabsButThis()
        {
            foreach (TabPage Page in tabControl1.TabPages)
            {
                if (Page.Name != tabControl1.SelectedTab.Name)
                {
                    tabControl1.TabPages.Remove(Page);
                }
            }
        }

        #endregion

        #region SaveAndOpen

        private void Save()
        {
            saveFileDialog1.FileName = tabControl1.SelectedTab.Name;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "RTF|*.rtf";
            saveFileDialog1.Title = "Save";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length > 0)
                {
                    GetCurrentDocument.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                }
            }
        }

        private void SaveAs()
        {
            saveFileDialog1.FileName = tabControl1.SelectedTab.Name;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "Text Files|*.txt|VB Files|*.vb|C# Files|*.cs|All Files|*.*";
            saveFileDialog1.Title = "Save As";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length > 0)
                {
                    GetCurrentDocument.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                }
            }
        }

        private void Open()
        {
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.Filter = "RTF|*.rtf|Text Files||*.txt|VB Files|*.vb|C# Files|*.cs|All Files|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openFileDialog1.FileName.Length > 9)
                {
                    GetCurrentDocument.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                }
            }
        }

        #endregion

        #region Context

        private void Undo()
        {
            GetCurrentDocument.Undo();
        }

        private void Redo()
        {
            GetCurrentDocument.Redo();
        }

        private void Cut()
        {
            GetCurrentDocument.Cut();
        }

        private void Copy()
        {
            GetCurrentDocument.Copy();
        }

        private void Paste()
        {
            GetCurrentDocument.Paste();
        }

        private void SelectAll()
        {
            GetCurrentDocument.SelectAll();
        }

        #endregion

        #region Fonts

        private void InvertFontStyle(FontStyle style)
        {
            Font StyledFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, GetCurrentDocument.SelectionFont.Style | style);
            Font NoStyledFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, GetCurrentDocument.SelectionFont.Style ^ style);

            if (GetCurrentDocument.SelectionFont.Style.HasFlag(style))
            {
                GetCurrentDocument.SelectionFont = NoStyledFont;
            } else
            {
                GetCurrentDocument.SelectionFont = StyledFont;
            }
        }

        #endregion

        #endregion

        #region General

        private void PopulateFontSizes()
        {
            for (int i = 1; i <= 75; i++)
            {
                toolStripComboBox2.Items.Add(i);
            }

            toolStripComboBox2.SelectedIndex = 11;
        }

        #endregion

        #region Properties

        private RichTextBox GetCurrentDocument
        {
            get { return (RichTextBox)tabControl1.SelectedTab.Controls["Body"]; }
        }


        #endregion

        #region ContextMenu

        //Adding Context Menu Item Functions
        private void ContextUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void ContextRedo_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void ContextCut_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void ContextCopy_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void ContextPaste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void ContextSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void ContextClose_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        private void ContextClose_All_Click(object sender, EventArgs e)
        {
            RemoveAllTabs();
        }

        private void ContextClose_All_But_This_Click(object sender, EventArgs e)
        {
            RemoveAllTabsButThis();
        }

        #endregion

        #region MenuItems
        //Making the Menu Buttons at the top functional (File, Edit)

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

        #endregion

        #region TopToolStrip
        //Contains all actions and methods for the top tool strip below the menu items.

        #region StyleButtons
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            InvertFontStyle(FontStyle.Bold);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            InvertFontStyle(FontStyle.Italic);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            InvertFontStyle(FontStyle.Underline);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            InvertFontStyle(FontStyle.Strikeout);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectedText = GetCurrentDocument.SelectedText.ToUpper();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectedText = GetCurrentDocument.SelectedText.ToLower();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            float NewFontSize = GetCurrentDocument.SelectionFont.SizeInPoints + 2;

            Font NewSize = new Font(GetCurrentDocument.SelectionFont.Name, NewFontSize, GetCurrentDocument.SelectionFont.Style);

            GetCurrentDocument.SelectionFont = NewSize;
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            float NewFontSize = GetCurrentDocument.SelectionFont.SizeInPoints - 2;

            Font NewSize = new Font(GetCurrentDocument.SelectionFont.Name, NewFontSize, GetCurrentDocument.SelectionFont.Style);

            GetCurrentDocument.SelectionFont = NewSize;
        }

        #endregion

        #region HighlightButton

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetCurrentDocument.SelectionColor = colorDialog1.Color;
            }
        }

        private void HighlightGreen_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectionBackColor = HighlightGreen.BackColor;
        }

        private void HighlightOrange_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectionBackColor = HighlightOrange.BackColor;
        }

        private void HighlightYellow_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectionBackColor = HighlightYellow.BackColor;
        }


        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectionBackColor = toolStripDropDownButton1.BackColor;
        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectionBackColor = defaultToolStripMenuItem.BackColor;
        }

        #endregion

        #region DropDownBox1

        InstalledFontCollection InsFonts = new InstalledFontCollection();

        private void DrawComboBox1Items()
        {
            toolStripComboBox1.ComboBox.DrawMode = DrawMode.OwnerDrawVariable;
            List<FontFamily> FamilyList = new List<FontFamily>();
            foreach (FontFamily item in InsFonts.Families)
            {
                FamilyList.Add(item);
            }
            toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            toolStripComboBox1.ComboBox.DataSource = FamilyList;
            toolStripComboBox1.ComboBox.DrawItem += new DrawItemEventHandler(toolStripComboBox1_DrawItem);
            toolStripComboBox1.ComboBox.MeasureItem += new MeasureItemEventHandler(toolStripComboBox1_MeasureItem);
        }

        private void toolStripComboBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 23;
            e.ItemWidth = 260;
        }

        private void toolStripComboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font MyFont;
            List<FontFamily> FamilyList = new List<FontFamily>();
            foreach (FontFamily item in InsFonts.Families)
            {
                FamilyList.Add(item);
            }
            e.DrawBackground();
            Rectangle rectangle = new Rectangle(2, e.Bounds.Top + 2,
            e.Bounds.Height + 2, e.Bounds.Height - 4);
            e.Graphics.FillRectangle(Brushes.White, rectangle);

            MyFont = new Font(FamilyList[e.Index].Name, 12, FontStyle.Regular);

            e.Graphics.DrawString(FamilyList[e.Index].Name, MyFont, Brushes.Black, new RectangleF(e.Bounds.X + rectangle.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Font NewFont = new Font(toolStripComboBox1.SelectedItem.ToString(), GetCurrentDocument.SelectionFont.Size, GetCurrentDocument.SelectionFont.Style);

            if (GetCurrentDocument.SelectedText != "")
            {
                GetCurrentDocument.SelectionFont = NewFont;
            } else
            {
                GetCurrentDocument.Font = NewFont;
            }

        }

        #endregion

        #region DropDownBox2

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            float NewSize;

            float.TryParse(toolStripComboBox2.SelectedItem.ToString(), out NewSize);

            Font NewFont = new Font(GetCurrentDocument.SelectionFont.Name, NewSize, GetCurrentDocument.SelectionFont.Style);

            GetCurrentDocument.SelectionFont = NewFont;
            GetCurrentDocument.Font = NewFont;
        }
        #endregion

        #endregion

        #region LeftToolStrip
        //Contains all the funcions for the left tool strip

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void RemoveTabToolStripButton_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        #endregion

        #region Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (GetCurrentDocument.Text.Length > 0)
            {
                toolStripStatusLabel1.Text = GetCurrentDocument.Text.Length.ToString();
            }
            else
            {
                toolStripStatusLabel1.Text = 0.ToString();
            }
        }

        #endregion
    }
}
