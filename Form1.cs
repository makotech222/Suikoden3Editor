using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Suikoden3Editor
{
    public partial class MainForm : Form
    {
        public List<ComboBox> _statComboBoxes = new List<ComboBox>();
        public List<TextBox> _runeTextBoxes = new List<TextBox>();
        public List<ComboBox> _skillComboBoxes = new List<ComboBox>();
        public List<Character> _characters = new List<Character>();
        public List<TextBox> _fixedSkillTextBoxes = new List<TextBox>();
        public ISOData _isoData;
        private bool _stopUpdate = false;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void MainForm_Load(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Suikoden 3 ISO File";
            fdlg.Filter = "ISO Files (*.iso)|*.iso";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                _isoData = new ISOData(fdlg.FileName);
                LoadFormControls();
            }
            else
            {
                this.Close();
            }
        }

        private void cbCharacterName_SelectedIndexChanged(object sender, EventArgs e)
        {
            _stopUpdate = true;
            RefreshCharacter();
            _stopUpdate = false;
        }

        private void statChange(object sender, EventArgs e)
        {
            if (_stopUpdate)
                return;
            UpdateCharacter();
        }

        private void btnMaxAll_Click(object sender, EventArgs e)
        {
            _stopUpdate = true;
            foreach(var character in _characters)
            {
                character.HPGrowth = 11;
                character.PWRGrowth = 11;
                character.SKLGrowth = 11;
                character.MAGGrowth = 11;
                character.REPGrowth = 11;
                //character.PDFGrowth = 11;
                character.MDFGrowth = 11;
                character.SPDGrowth = 11;
                character.LUKGrowth = 11;
                character.HeadRuneLevel = 1;
                character.RHRuneLevel = 1;
                character.LHRuneLevel = 1;
                for (int i = 0; i < _skillComboBoxes.Count; i++)
                {
                    character.SkillLevels[i] = 7;
                }
                for (int i = 0; i < character.FixedSkills.Count; i++)
                {
                    character.FixedSkills[i] = 0;
                }
                character.FreeSkillsSlotCount = 8;
                character.UpdateRawData();
            }
            this.RefreshCharacter();
            _stopUpdate = false;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            _isoData.Save(this._characters);
            MessageBox.Show("Save Successful");
        }

        #endregion Event Handlers

        #region Methods

        private void LoadFormControls()
        {
            var options = JsonSerializer.Deserialize<Options>(File.ReadAllText("Options.json"));
            int index = 0;
            var statSelections = new List<object>()
            {
                new { Name = "No Growth (0)",Value = 0 },
                new { Name = "F (1)",Value = 1 },
                new { Name = "E (2)",Value = 2 },
                new { Name = "D (3)",Value = 3 },
                new { Name = "C (4)",Value = 4 },
                new { Name = "B (5)",Value = 5 },
                new { Name = "A (6)",Value = 6 },
                new { Name = "S (7)",Value = 7 },
                new { Name = "S+ (8)",Value = 8 },
                new { Name = "S++ (9)",Value = 9 },
                new { Name = "S+++ (10)",Value = 10 },
                new { Name = "S++++ (11)",Value = 11 },
            };
            foreach (var statOption in options.Stats)
            {
                var combobox = new ComboBox();
                combobox.Name = "cb" + statOption.Name.Replace(" ", "").Trim();
                combobox.Location = new Point(117, 57 + (index * 30));
                combobox.ValueMember = "Value";
                combobox.DisplayMember = "Name";
                combobox.Items.AddRange(statSelections.ToArray());
                combobox.SelectedIndexChanged += statChange;
                var label = new Label();
                label.Name = "lbl" + statOption.Name.Replace(" ", "").Trim();
                label.Location = new Point(9, 60 + (index * 30));
                label.Text = statOption.Name;
                this.Controls.Add(label);
                this.Controls.Add(combobox);
                _statComboBoxes.Add(combobox);
                index++;
            }
            index++;
            foreach (var runeOption in options.Runes)
            {
                var textBox = new TextBox();
                textBox.Name = "cb" + runeOption.Name.Replace(" ", "").Trim();
                textBox.Location = new Point(117, 57 + (index * 30));
                textBox.TextChanged += statChange;
                var label = new Label();
                label.Name = "lbl" + runeOption.Name.Replace(" ", "").Trim();
                label.Location = new Point(9, 60 + (index * 30));
                label.Text = runeOption.Name;
                this.Controls.Add(label);
                this.Controls.Add(textBox);
                _runeTextBoxes.Add(textBox);
                index++;
            }
            index = 0;
            int column = 0;
            var skillSelections = new List<object>()
            {
                new { Name = "Not Available",Value = "0" },
                new { Name = "Slow Max of A+ (?)",Value = "1" },
                new { Name = "INVALID",Value = "2" },
                new { Name = "C",Value = "3" },
                new { Name = "B",Value = "4" },
                new { Name = "B+ (?)",Value = "5" },
                new { Name = "A",Value = "6" },
                new { Name = "S",Value = "7" }
            };
            foreach (var skillOption in options.Skills)
            {
                var combobox = new ComboBox();
                combobox.Name = "cb" + skillOption.Name.Replace(" ", "").Trim();
                combobox.Location = new Point(450 + (column * 300), 57 + (index * 30));
                combobox.ValueMember = "Value";
                combobox.DisplayMember = "Name";
                combobox.Items.AddRange(skillSelections.ToArray());
                combobox.SelectedIndexChanged += statChange;
                var label = new Label();
                label.Name = "lbl" + skillOption.Name.Replace(" ", "").Trim();
                label.Location = new Point(288 + (column * 300), 60 + (index * 30));
                label.Text = skillOption.Name;
                this.Controls.Add(label);
                this.Controls.Add(combobox);
                _skillComboBoxes.Add(combobox);
                index++;
                if (index > 15)
                {
                    index = 0;
                    column++;
                }
            }


            index = 0;
            foreach (var fixedSkillOption in options.FixedSkill)
            {
                var textBox = new TextBox();
                textBox.Name = "cb" + fixedSkillOption.Name.Replace(" ", "").Trim();
                textBox.Location = new Point(1350, 57 + (index * 30));
                textBox.TextChanged += statChange;
                var label = new Label();
                label.Name = "lbl" + fixedSkillOption.Name.Replace(" ", "").Trim();
                label.Location = new Point(1200, 60 + (index * 30));
                label.Text = fixedSkillOption.Name;
                label.Width = 150;
                this.Controls.Add(label);
                this.Controls.Add(textBox);
                _fixedSkillTextBoxes.Add(textBox);
                index++;
            }

            _characters = GetCharacters(File.ReadAllLines("Characters.txt").ToList());
            foreach (var character in _characters)
            {
                cbCharacterName.Items.Add(new { Name = character.Name, Value = character.Offset });
            }
            cbCharacterName.ValueMember = "Value";
            cbCharacterName.DisplayMember = "Name";
        }

        private List<Character> GetCharacters(List<string> characterFile)
        {
            var result = new List<Character>();
            foreach (var c in characterFile)
            {
                var split = c.Split(":");
                var offset = Convert.ToInt64(split[0], 16);
                var name = split[1];
                result.Add(_isoData.GetCharacter(offset, name));
            }
            return result;
        }

        private void RefreshCharacter()
        {
            var selectedCharacter = _characters.First(x => x.Offset == ((dynamic)cbCharacterName.SelectedItem).Value);
            _statComboBoxes[0].SelectedIndex = selectedCharacter.HPGrowth;
            _statComboBoxes[1].SelectedIndex = selectedCharacter.PWRGrowth;
            _statComboBoxes[2].SelectedIndex = selectedCharacter.SKLGrowth;
            _statComboBoxes[3].SelectedIndex = selectedCharacter.MAGGrowth;
            _statComboBoxes[4].SelectedIndex = selectedCharacter.REPGrowth;
            _statComboBoxes[5].SelectedIndex = selectedCharacter.PDFGrowth;
            _statComboBoxes[6].SelectedIndex = selectedCharacter.MDFGrowth;
            _statComboBoxes[7].SelectedIndex = selectedCharacter.SPDGrowth;
            _statComboBoxes[8].SelectedIndex = selectedCharacter.LUKGrowth;

            _runeTextBoxes[0].Text = selectedCharacter.HeadRuneLevel.ToString();
            _runeTextBoxes[1].Text = selectedCharacter.RHRuneLevel.ToString();
            _runeTextBoxes[2].Text = selectedCharacter.LHRuneLevel.ToString();

            for (int i = 0; i < _skillComboBoxes.Count; i++)
            {
                _skillComboBoxes[i].SelectedIndex = selectedCharacter.SkillLevels[i];
            }
            for (int i = 0; i < _fixedSkillTextBoxes.Count; i++)
            {
                if (i == _fixedSkillTextBoxes.Count - 1)
                    _fixedSkillTextBoxes[i].Text = selectedCharacter.FreeSkillsSlotCount.ToString();
                else
                    _fixedSkillTextBoxes[i].Text = selectedCharacter.FixedSkills[i].ToString();
            }
            lblRaw.Text = "Raw Data: " + String.Join(' ', selectedCharacter.RawData.Take(50).Select(x => x.ToString("X2")));
        }

        private void UpdateCharacter()
        {
            var selectedCharacter = _characters.First(x => x.Offset == ((dynamic)cbCharacterName.SelectedItem).Value);
            selectedCharacter.HPGrowth = (byte)_statComboBoxes[0].SelectedIndex;
            selectedCharacter.PWRGrowth = (byte)_statComboBoxes[1].SelectedIndex;
            selectedCharacter.SKLGrowth = (byte)_statComboBoxes[2].SelectedIndex;
            selectedCharacter.MAGGrowth = (byte)_statComboBoxes[3].SelectedIndex;
            selectedCharacter.REPGrowth = (byte)_statComboBoxes[4].SelectedIndex;
            selectedCharacter.PDFGrowth = (byte)_statComboBoxes[5].SelectedIndex;
            selectedCharacter.MDFGrowth = (byte)_statComboBoxes[6].SelectedIndex;
            selectedCharacter.SPDGrowth = (byte)_statComboBoxes[7].SelectedIndex;
            selectedCharacter.LUKGrowth = (byte)_statComboBoxes[8].SelectedIndex;
            selectedCharacter.HeadRuneLevel = Convert.ToByte(_runeTextBoxes[0].Text);
            selectedCharacter.RHRuneLevel = Convert.ToByte(_runeTextBoxes[1].Text);
            selectedCharacter.LHRuneLevel = Convert.ToByte(_runeTextBoxes[2].Text);
            for (int i = 0; i < _skillComboBoxes.Count; i++)
            {
                selectedCharacter.SkillLevels[i] = Convert.ToByte(_skillComboBoxes[i].SelectedIndex);
            }
            for (int i = 0; i < _fixedSkillTextBoxes.Count; i++)
            {
                if (i == _fixedSkillTextBoxes.Count - 1)
                    selectedCharacter.FreeSkillsSlotCount = Convert.ToByte(_fixedSkillTextBoxes[i].Text);
                else
                    selectedCharacter.FixedSkills[i] = Convert.ToByte(_fixedSkillTextBoxes[i].Text);
            }
            selectedCharacter.UpdateRawData();
        }

        #endregion Methods

    }

    public class Options
    {
        public List<Option> Stats { get; set; }
        public List<Option> Runes { get; set; }
        public List<Option> Skills { get; set; }
        public List<Option> FixedSkill { get; set; }

        public class Option
        {
            public string Name { get; set; }
        }
    }

    public class ISOData
    {
        private const long _maxRange = 0x00000084;

        /// <summary>
        /// Hugo's stats start here (HP Growth)
        /// </summary>
        private const long _initialAddress = 0x3e13bc;
        private const long _finalAddress = _initialAddress + 0x00002838 + _maxRange;
        private byte[] _rawData;
        private string _filePath;

        public ISOData(string filePath)
        {
            _rawData = ReadBlock(filePath, _initialAddress, (_finalAddress - _initialAddress));
            _filePath = filePath;
        }

        public Character GetCharacter(long offset, string name)
        {
            byte[] data = new byte[_maxRange];
            Array.Copy(_rawData, offset, data, 0, _maxRange);
            var result = new Character(data, name, offset);
            return result;
        }

        public void Save(List<Character> characters)
        {
            foreach(var character in characters)
            {
                WriteBlock(_filePath, _initialAddress + character.Offset, character.RawData);
            }
        }

        private static byte[] ReadBlock(string fileName, long offset, long length)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(offset, SeekOrigin.Begin);
                    byte[] data = new byte[length];
                    fs.Read(data, 0, (int)length);
                    return data;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception reading ISO : " + e.Message);
            }
            return null;
        }

        public static void WriteBlock(string fileName, long offset, byte[] data)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    fs.Seek(offset, SeekOrigin.Begin);
                    fs.Write(data, 0, (int)data.Length);
                    fs.Flush();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception Writing ISO : " + e.Message);
            }
        }
    }

    public class Character
    {
        public string Name { get; set; }
        public long Offset { get; set; }
        public byte[] RawData { get; set; }

        public byte HPGrowth { get; set; }
        public byte PWRGrowth { get; set; }
        public byte SKLGrowth { get; set; }
        public byte MAGGrowth { get; set; }
        public byte REPGrowth { get; set; }
        public byte PDFGrowth { get; set; }
        public byte MDFGrowth { get; set; }
        public byte SPDGrowth { get; set; }
        public byte LUKGrowth { get; set; }

        public byte HeadRuneLevel { get; set; }
        public byte RHRuneLevel { get; set; }
        public byte LHRuneLevel { get; set; }

        public List<byte> SkillLevels { get; set; } = new List<byte>();
        public List<byte> FixedSkills { get; set; } = new List<byte>();
        public byte FreeSkillsSlotCount { get; set; }

        public Character(byte[] data, string name, long offset)
        {
            this.RawData = data;
            this.Name = name;
            this.Offset = offset;

            this.HPGrowth = data[0];
            this.PWRGrowth = data[4];
            this.SKLGrowth = data[5];
            this.MAGGrowth = data[6];
            this.REPGrowth = data[7];
            this.PDFGrowth = data[8];
            this.MDFGrowth = data[9];
            this.SPDGrowth = data[10];
            this.LUKGrowth = data[11];
            this.HeadRuneLevel = data[13];
            this.RHRuneLevel = data[14];
            this.LHRuneLevel = data[15];
            for (int i = 0; i < 53; i++)
            {
                this.SkillLevels.Add(data[16 + i]);
            }
            for (int i = 0; i < 16; i++)
            {
                this.FixedSkills.Add(data[80 + i]);
            }
            FreeSkillsSlotCount = data[96];
        }

        public void UpdateRawData()
        {
            RawData[0] = this.HPGrowth;
            RawData[4] = this.PWRGrowth;
            RawData[5] = this.SKLGrowth;
            RawData[6] = this.MAGGrowth;
            RawData[7] = this.REPGrowth;
            RawData[8] = this.PDFGrowth;
            RawData[9] = this.MDFGrowth;
            RawData[10] = this.SPDGrowth;
            RawData[11] = this.LUKGrowth;
            RawData[13] = this.HeadRuneLevel;
            RawData[14] = this.RHRuneLevel;
            RawData[15] = this.LHRuneLevel;
            for (int i = 0; i < SkillLevels.Count; i++)
            {
                RawData[16 + i] = this.SkillLevels[i];
            }
            for (int i = 0; i < FixedSkills.Count; i++)
            {
                RawData[80 + i] = this.FixedSkills[i];
            }
            RawData[96] = FreeSkillsSlotCount;
        }
    }
}