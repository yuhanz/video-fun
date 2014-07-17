namespace VideoFun
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.displayPictureBox = new System.Windows.Forms.PictureBox();
            this.openVideoButton = new System.Windows.Forms.Button();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.SaveVideoButton = new System.Windows.Forms.Button();
            this.timeTrackBar = new System.Windows.Forms.TrackBar();
            this.startTimeButton = new System.Windows.Forms.Button();
            this.endTimeButton = new System.Windows.Forms.Button();
            this.displayLabel = new System.Windows.Forms.Label();
            this.debugButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.trackButton = new System.Windows.Forms.Button();
            this.saveFramesButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.displayPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // displayPictureBox
            // 
            this.displayPictureBox.Location = new System.Drawing.Point(25, 12);
            this.displayPictureBox.Name = "displayPictureBox";
            this.displayPictureBox.Size = new System.Drawing.Size(970, 611);
            this.displayPictureBox.TabIndex = 0;
            this.displayPictureBox.TabStop = false;
            this.displayPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.displayPictureBox_MouseMove);
            this.displayPictureBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.displayPictureBox_DragDrop);
            this.displayPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.displayPictureBox_MouseDown);
            this.displayPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.displayPictureBox_MouseUp);
            this.displayPictureBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.displayPictureBox_DragEnter);
            // 
            // openVideoButton
            // 
            this.openVideoButton.Location = new System.Drawing.Point(12, 692);
            this.openVideoButton.Name = "openVideoButton";
            this.openVideoButton.Size = new System.Drawing.Size(75, 23);
            this.openVideoButton.TabIndex = 1;
            this.openVideoButton.Text = "Open Video";
            this.openVideoButton.UseVisualStyleBackColor = true;
            this.openVideoButton.Click += new System.EventHandler(this.openVideoButton_Click);
            // 
            // statusTextBox
            // 
            this.statusTextBox.AllowDrop = true;
            this.statusTextBox.Location = new System.Drawing.Point(433, 681);
            this.statusTextBox.Multiline = true;
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.statusTextBox.Size = new System.Drawing.Size(243, 63);
            this.statusTextBox.TabIndex = 2;
            this.statusTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.statusTextBox_DragDrop);
            this.statusTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.statusTextBox_DragEnter);
            // 
            // SaveVideoButton
            // 
            this.SaveVideoButton.Location = new System.Drawing.Point(12, 721);
            this.SaveVideoButton.Name = "SaveVideoButton";
            this.SaveVideoButton.Size = new System.Drawing.Size(75, 23);
            this.SaveVideoButton.TabIndex = 3;
            this.SaveVideoButton.Text = "Save Video";
            this.SaveVideoButton.UseVisualStyleBackColor = true;
            this.SaveVideoButton.Click += new System.EventHandler(this.SaveVideoButton_Click);
            // 
            // timeTrackBar
            // 
            this.timeTrackBar.AccessibleDescription = "123";
            this.timeTrackBar.BackColor = System.Drawing.SystemColors.Control;
            this.timeTrackBar.LargeChange = 7;
            this.timeTrackBar.Location = new System.Drawing.Point(12, 630);
            this.timeTrackBar.Maximum = 0;
            this.timeTrackBar.Name = "timeTrackBar";
            this.timeTrackBar.Size = new System.Drawing.Size(907, 45);
            this.timeTrackBar.TabIndex = 4;
            this.timeTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.timeTrackBar.ValueChanged += new System.EventHandler(this.timeTrackBar_ValueChanged);
            // 
            // startTimeButton
            // 
            this.startTimeButton.Location = new System.Drawing.Point(332, 681);
            this.startTimeButton.Name = "startTimeButton";
            this.startTimeButton.Size = new System.Drawing.Size(75, 23);
            this.startTimeButton.TabIndex = 5;
            this.startTimeButton.Text = "start time";
            this.startTimeButton.UseVisualStyleBackColor = true;
            this.startTimeButton.Click += new System.EventHandler(this.startTimeButton_Click);
            // 
            // endTimeButton
            // 
            this.endTimeButton.Location = new System.Drawing.Point(332, 709);
            this.endTimeButton.Name = "endTimeButton";
            this.endTimeButton.Size = new System.Drawing.Size(75, 23);
            this.endTimeButton.TabIndex = 6;
            this.endTimeButton.Text = "end time";
            this.endTimeButton.UseVisualStyleBackColor = true;
            this.endTimeButton.Click += new System.EventHandler(this.endTimeButton_Click);
            // 
            // displayLabel
            // 
            this.displayLabel.AutoSize = true;
            this.displayLabel.Location = new System.Drawing.Point(22, 662);
            this.displayLabel.Name = "displayLabel";
            this.displayLabel.Size = new System.Drawing.Size(13, 13);
            this.displayLabel.TabIndex = 7;
            this.displayLabel.Text = "0";
            // 
            // debugButton
            // 
            this.debugButton.Location = new System.Drawing.Point(191, 721);
            this.debugButton.Name = "debugButton";
            this.debugButton.Size = new System.Drawing.Size(82, 23);
            this.debugButton.TabIndex = 8;
            this.debugButton.Text = "Debug";
            this.debugButton.UseVisualStyleBackColor = true;
            this.debugButton.Click += new System.EventHandler(this.debugButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(191, 695);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(21, 20);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "0.5";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(218, 695);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(21, 20);
            this.textBox2.TabIndex = 10;
            this.textBox2.Text = "0.5";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(245, 695);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(21, 20);
            this.textBox3.TabIndex = 11;
            this.textBox3.Text = "0.5";
            // 
            // trackButton
            // 
            this.trackButton.Location = new System.Drawing.Point(277, 681);
            this.trackButton.Name = "trackButton";
            this.trackButton.Size = new System.Drawing.Size(49, 23);
            this.trackButton.TabIndex = 12;
            this.trackButton.Text = "Track";
            this.trackButton.UseVisualStyleBackColor = true;
            this.trackButton.Click += new System.EventHandler(this.trackButton_Click);
            // 
            // saveFramesButton
            // 
            this.saveFramesButton.Location = new System.Drawing.Point(93, 722);
            this.saveFramesButton.Name = "saveFramesButton";
            this.saveFramesButton.Size = new System.Drawing.Size(75, 23);
            this.saveFramesButton.TabIndex = 13;
            this.saveFramesButton.Text = "Save frames";
            this.saveFramesButton.UseVisualStyleBackColor = true;
            this.saveFramesButton.Click += new System.EventHandler(this.saveFramesButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.AllowDrop = true;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(754, 662);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 95);
            this.listBox1.TabIndex = 14;
            this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
            this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
            // 
            // MainForm
            // 
            this.AccessibleName = "";
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 764);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.saveFramesButton);
            this.Controls.Add(this.trackButton);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.debugButton);
            this.Controls.Add(this.displayLabel);
            this.Controls.Add(this.endTimeButton);
            this.Controls.Add(this.startTimeButton);
            this.Controls.Add(this.timeTrackBar);
            this.Controls.Add(this.SaveVideoButton);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.openVideoButton);
            this.Controls.Add(this.displayPictureBox);
            this.Name = "MainForm";
            this.Text = "Video Fun";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            ((System.ComponentModel.ISupportInitialize)(this.displayPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox displayPictureBox;
        private System.Windows.Forms.Button openVideoButton;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.Button SaveVideoButton;
        private System.Windows.Forms.TrackBar timeTrackBar;
        private System.Windows.Forms.Button startTimeButton;
        private System.Windows.Forms.Button endTimeButton;
        private System.Windows.Forms.Label displayLabel;
        private System.Windows.Forms.Button debugButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button trackButton;
        private System.Windows.Forms.Button saveFramesButton;
        private System.Windows.Forms.ListBox listBox1;
    }
}

