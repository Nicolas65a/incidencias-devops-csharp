namespace GestionIncidencias.Formularios
{
    partial class frmAdmin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpDatos = new System.Windows.Forms.GroupBox();
            this.lblEdicion = new System.Windows.Forms.Label();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblTituloCampo = new System.Windows.Forms.Label();
            this.txtTitulo = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblResponsable = new System.Windows.Forms.Label();
            this.txtResponsable = new System.Windows.Forms.TextBox();
            this.lblPrioridad = new System.Windows.Forms.Label();
            this.cmbPrioridad = new System.Windows.Forms.ComboBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.grpDatos.Location = new System.Drawing.Point(24, 20);
            this.grpDatos.Size = new System.Drawing.Size(432, 515);
            this.grpDatos.Name = "grpDatos";
            this.grpDatos.Text = "Datos de la incidencia";
            this.lblEdicion.Location = new System.Drawing.Point(20, 30);
            this.lblEdicion.Size = new System.Drawing.Size(388, 25);
            this.lblEdicion.Name = "lblEdicion";
            this.lblEdicion.Text = "Nueva incidencia";
            this.lblCodigo.Location = new System.Drawing.Point(20, 67);
            this.lblCodigo.Size = new System.Drawing.Size(95, 25);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Text = "Código";
            this.txtCodigo.Location = new System.Drawing.Point(140, 63);
            this.txtCodigo.Size = new System.Drawing.Size(268, 28);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Text = "Automático";
            this.txtCodigo.ReadOnly = true;
            this.lblTituloCampo.Location = new System.Drawing.Point(20, 108);
            this.lblTituloCampo.Size = new System.Drawing.Size(388, 25);
            this.lblTituloCampo.Name = "lblTituloCampo";
            this.lblTituloCampo.Text = "Título del problema";
            this.txtTitulo.Location = new System.Drawing.Point(20, 138);
            this.txtTitulo.Size = new System.Drawing.Size(388, 28);
            this.txtTitulo.Name = "txtTitulo";
            this.txtTitulo.MaxLength = 100;
            this.lblDescripcion.Location = new System.Drawing.Point(20, 179);
            this.lblDescripcion.Size = new System.Drawing.Size(388, 25);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Text = "Descripción";
            this.txtDescripcion.Location = new System.Drawing.Point(20, 209);
            this.txtDescripcion.Size = new System.Drawing.Size(388, 95);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescripcion.MaxLength = 1000;
            this.lblResponsable.Location = new System.Drawing.Point(20, 317);
            this.lblResponsable.Size = new System.Drawing.Size(388, 25);
            this.lblResponsable.Name = "lblResponsable";
            this.lblResponsable.Text = "Responsable";
            this.txtResponsable.Location = new System.Drawing.Point(20, 347);
            this.txtResponsable.Size = new System.Drawing.Size(388, 28);
            this.txtResponsable.Name = "txtResponsable";
            this.txtResponsable.MaxLength = 80;
            this.lblPrioridad.Location = new System.Drawing.Point(20, 389);
            this.lblPrioridad.Size = new System.Drawing.Size(105, 25);
            this.lblPrioridad.Name = "lblPrioridad";
            this.lblPrioridad.Text = "Prioridad";
            this.cmbPrioridad.Location = new System.Drawing.Point(140, 385);
            this.cmbPrioridad.Size = new System.Drawing.Size(268, 30);
            this.cmbPrioridad.Name = "cmbPrioridad";
            this.cmbPrioridad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrioridad.Items.AddRange(new object[] { "Alta", "Media", "Baja" });
            this.btnGuardar.Location = new System.Drawing.Point(20, 450);
            this.btnGuardar.Size = new System.Drawing.Size(185, 38);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Text = "Guardar datos";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            this.grpDatos.Controls.Add(this.lblEdicion);
            this.grpDatos.Controls.Add(this.lblCodigo);
            this.grpDatos.Controls.Add(this.txtCodigo);
            this.grpDatos.Controls.Add(this.lblTituloCampo);
            this.grpDatos.Controls.Add(this.txtTitulo);
            this.grpDatos.Controls.Add(this.lblDescripcion);
            this.grpDatos.Controls.Add(this.txtDescripcion);
            this.grpDatos.Controls.Add(this.lblResponsable);
            this.grpDatos.Controls.Add(this.txtResponsable);
            this.grpDatos.Controls.Add(this.lblPrioridad);
            this.grpDatos.Controls.Add(this.cmbPrioridad);
            this.grpDatos.Controls.Add(this.btnGuardar);
            this.btnCancelar.Location = new System.Drawing.Point(223, 450);
            this.btnCancelar.Size = new System.Drawing.Size(185, 38);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            this.grpDatos.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.grpDatos);
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 560);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.AcceptButton = this.btnGuardar;
            this.CancelButton = this.btnCancelar;
            this.Name = "frmAdmin";
            this.Text = "Ingresar incidencia";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox grpDatos;
        private System.Windows.Forms.Label lblEdicion;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblTituloCampo;
        private System.Windows.Forms.TextBox txtTitulo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblResponsable;
        private System.Windows.Forms.TextBox txtResponsable;
        private System.Windows.Forms.Label lblPrioridad;
        private System.Windows.Forms.ComboBox cmbPrioridad;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
