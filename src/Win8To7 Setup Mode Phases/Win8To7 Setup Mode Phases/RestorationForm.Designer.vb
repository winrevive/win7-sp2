<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RestorationForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RestorationForm))
        Me.LabelStatus = New System.Windows.Forms.Label()
        Me.FakeIntro = New System.Windows.Forms.Panel()
        Me.ProgressAnim = New System.Windows.Forms.Panel()
        Me.ProgressAnimFill = New System.Windows.Forms.PictureBox()
        Me.CustomisingMode = New System.Windows.Forms.Panel()
        Me.CustomisingLogo = New System.Windows.Forms.PictureBox()
        Me.CustomisingStatus = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ProgressAnim.SuspendLayout()
        CType(Me.ProgressAnimFill, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CustomisingMode.SuspendLayout()
        CType(Me.CustomisingLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelStatus
        '
        Me.LabelStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelStatus.BackColor = System.Drawing.Color.Transparent
        Me.LabelStatus.Location = New System.Drawing.Point(0, 505)
        Me.LabelStatus.Name = "LabelStatus"
        Me.LabelStatus.Size = New System.Drawing.Size(863, 152)
        Me.LabelStatus.TabIndex = 1
        Me.LabelStatus.Text = "Windows is being restored to how it was before it was transformed." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Windows wil" & _
    "l restart several times during this process, do not turn off your computer durin" & _
    "g this time. "
        Me.LabelStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'FakeIntro
        '
        Me.FakeIntro.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FakeIntro.Location = New System.Drawing.Point(0, 0)
        Me.FakeIntro.Name = "FakeIntro"
        Me.FakeIntro.Size = New System.Drawing.Size(863, 680)
        Me.FakeIntro.TabIndex = 3
        '
        'ProgressAnim
        '
        Me.ProgressAnim.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressAnim.BackColor = System.Drawing.Color.Gray
        Me.ProgressAnim.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ProgressAnim.Controls.Add(Me.ProgressAnimFill)
        Me.ProgressAnim.Location = New System.Drawing.Point(0, 655)
        Me.ProgressAnim.Name = "ProgressAnim"
        Me.ProgressAnim.Size = New System.Drawing.Size(863, 25)
        Me.ProgressAnim.TabIndex = 5
        '
        'ProgressAnimFill
        '
        Me.ProgressAnimFill.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressAnimFill.BackColor = System.Drawing.Color.GreenYellow
        Me.ProgressAnimFill.Location = New System.Drawing.Point(1, 1)
        Me.ProgressAnimFill.Name = "ProgressAnimFill"
        Me.ProgressAnimFill.Size = New System.Drawing.Size(1, 18)
        Me.ProgressAnimFill.TabIndex = 0
        Me.ProgressAnimFill.TabStop = False
        '
        'CustomisingMode
        '
        Me.CustomisingMode.BackColor = System.Drawing.Color.Transparent
        Me.CustomisingMode.Controls.Add(Me.CustomisingLogo)
        Me.CustomisingMode.Controls.Add(Me.CustomisingStatus)
        Me.CustomisingMode.Controls.Add(Me.PictureBox1)
        Me.CustomisingMode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CustomisingMode.Location = New System.Drawing.Point(0, 0)
        Me.CustomisingMode.Name = "CustomisingMode"
        Me.CustomisingMode.Size = New System.Drawing.Size(863, 680)
        Me.CustomisingMode.TabIndex = 6
        Me.CustomisingMode.Visible = False
        '
        'CustomisingLogo
        '
        Me.CustomisingLogo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CustomisingLogo.BackColor = System.Drawing.Color.Transparent
        Me.CustomisingLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.CustomisingLogo.Location = New System.Drawing.Point(12, 590)
        Me.CustomisingLogo.Name = "CustomisingLogo"
        Me.CustomisingLogo.Size = New System.Drawing.Size(839, 50)
        Me.CustomisingLogo.TabIndex = 5
        Me.CustomisingLogo.TabStop = False
        '
        'CustomisingStatus
        '
        Me.CustomisingStatus.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.CustomisingStatus.BackColor = System.Drawing.Color.Transparent
        Me.CustomisingStatus.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CustomisingStatus.Location = New System.Drawing.Point(73, 324)
        Me.CustomisingStatus.Name = "CustomisingStatus"
        Me.CustomisingStatus.Size = New System.Drawing.Size(716, 55)
        Me.CustomisingStatus.TabIndex = 4
        Me.CustomisingStatus.Text = "Customising transformation... Stage 0 of 4 - 0% complete." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Do not turn off your c" & _
    "omputer."
        Me.CustomisingStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = Global.Win8To7_Setup_Mode_Phases.My.Resources.Resources.progressspinner
        Me.PictureBox1.Location = New System.Drawing.Point(421, 290)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(20, 20)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'RestorationForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(863, 680)
        Me.Controls.Add(Me.FakeIntro)
        Me.Controls.Add(Me.CustomisingMode)
        Me.Controls.Add(Me.ProgressAnim)
        Me.Controls.Add(Me.LabelStatus)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RestorationForm"
        Me.Text = "Restoring - Win8To7 Transformation Pack Backend"
        Me.TopMost = True
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ProgressAnim.ResumeLayout(False)
        CType(Me.ProgressAnimFill, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CustomisingMode.ResumeLayout(False)
        CType(Me.CustomisingLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelStatus As System.Windows.Forms.Label
    Friend WithEvents FakeIntro As System.Windows.Forms.Panel
    Friend WithEvents ProgressAnim As System.Windows.Forms.Panel
    Friend WithEvents ProgressAnimFill As System.Windows.Forms.PictureBox
    Friend WithEvents CustomisingMode As System.Windows.Forms.Panel
    Friend WithEvents CustomisingLogo As System.Windows.Forms.PictureBox
    Friend WithEvents CustomisingStatus As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox

End Class
