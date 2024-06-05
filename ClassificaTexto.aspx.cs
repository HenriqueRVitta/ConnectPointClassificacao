using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Classificacao
{
    public partial class ClassificaTexto : System.Web.UI.Page
    {
        MySqlConnection conexao = new MySqlConnection(String.Format(ConfigurationManager.AppSettings["StrConnect"]));

        internal DataTable dtbN1 = null;
        internal DataTable dtbN2 = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dtbN1 = new DataTable();

                dtbN1 = CriaDataTableN1();

                Session["DataTableN1"] = dtbN1;

                dtbN2 = new DataTable();

                dtbN2 = CriaDataTableN2();

                Session["DataTableN2"] = dtbN2;

                conexao.Open();

                string SelC = "select ID as id,concat(Codigo,\" - \",Descricao) as nome from tb_classificacao where length(Codigo)=6 order by Codigo";
                MySqlCommand qrySelectC = new MySqlCommand(SelC, conexao);
                qrySelectC = new MySqlCommand(SelC, conexao);
                MySqlDataReader readerC = qrySelectC.ExecuteReader();

                ddlClassificacao.DataSource = readerC;
                ddlClassificacao.DataTextField = "nome";
                ddlClassificacao.DataValueField = "id";
                ddlClassificacao.DataBind();
                ddlClassificacao.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione a Classificação.", ""));

                qrySelectC.Dispose();

                conexao.Close();

                conexao.Open();

                string SelO = "select ID as id,Descricao as nome from tp_origem order by Descricao";
                MySqlCommand qrySelectO = new MySqlCommand(SelO, conexao);
                qrySelectO = new MySqlCommand(SelO, conexao);
                MySqlDataReader readerO = qrySelectO.ExecuteReader();

                ddlOrigem.DataSource = readerO;
                ddlOrigem.DataTextField = "nome";
                ddlOrigem.DataValueField = "id";
                ddlOrigem.DataBind();
                ddlOrigem.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione a Origem.", ""));

                qrySelectO.Dispose();

                conexao.Close();
            }
        }

        public void Carrega()
        {
            conexao.Open();

            string Sel = "SELECT a.ID,a.Objeto,DATE_FORMAT(a.DataCadastro, '%d/%m/%Y') as DataCadastro,b.Codigo,a.OrgaoResponsavel,c.Descricao from tb_edital as a left join tb_classificacao as b on a.ID_Classificacao = b.ID left join tp_origem as c on a.origem=c.ID  where ";

            if (rdbSim.Checked == true)
                Sel = Sel + " a.ID_Classificacao is not null ";
            else
                Sel = Sel + " a.ID_Classificacao is null ";

            if (ChbDelete.Checked == true)
            {
                Sel = Sel + " and a.DataExclusao between @dataini and @datafim and a.DataAprovacao is null and a.DataEdicao is null ";
                GrdEdital.Caption = "Editais Excluidos ";
            }
            else
            {
                if (ChbEditados.Checked == true)
                {
                    Sel = Sel + " and a.DataEdicao between @dataini and @datafim and a.DataExclusao is null and a.DataAprovacao is null ";
                    GrdEdital.Caption = "Editais Editados ";
                }
                else
                {
                    if (ChbAprovado.Checked == true)
                    {
                        Sel = Sel + "and a.DataAprovacao between @dataini and @datafim and a.DataEdicao is null and a.DataExclusao is null ";
                        GrdEdital.Caption = "Editais Aprovados ";
                    }
                    else
                    {
                        Sel = Sel + " and a.DataCadastro between @dataini and @datafim ";
                        GrdEdital.Caption = "Editais Cadastrados ";
                    }
                }
            }

            if (ddlOrigem.SelectedValue != "")
            {
                Sel = Sel + " and a.Origem=@origem";
            }

            if (rdbNao.Checked == true)
            {
                Sel = Sel + " and a.ID_Classificacao is null order by a.DataCadastro";
                GrdEdital.Caption = GrdEdital.Caption + "Não Classificados : ";
            }
            else
            {
                Sel = Sel + " and a.ID_Classificacao is not null order by a.DataCadastro";
                GrdEdital.Caption = GrdEdital.Caption + " Já Classificados : ";
            }

            MySqlCommand qrySelect = new MySqlCommand(Sel, conexao);
            qrySelect.Parameters.Add("@dataini", MySqlDbType.DateTime).Value = Convert.ToDateTime(TxtDataIni.Text + " 00:00:00");
            qrySelect.Parameters.Add("@datafim", MySqlDbType.DateTime).Value = Convert.ToDateTime(TxtDataFim.Text + " 23:59:59");
            if (ddlOrigem.SelectedValue != "")
                qrySelect.Parameters.Add("@origem", MySqlDbType.Int16).Value = Convert.ToInt16(ddlOrigem.SelectedValue);
            DataTable dataTable = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(qrySelect);
            da.Fill(dataTable);
            Session["TaskTable"] = dataTable;
            GrdEdital.DataSource = dataTable;
            GrdEdital.Caption = GrdEdital.Caption + Convert.ToString(dataTable.Rows.Count);
            GrdEdital.DataBind();
            ViewState["dirState"] = dataTable;
            ViewState["sortdr"] = "Asc";

            qrySelect.Dispose();

            conexao.Close();

            if (dataTable.Rows.Count == 0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Nenhum Item Foi Selecionado Para o Seu Filtro.');", true);
        }

        private void BindData()
        {
            GrdEdital.DataSource = Session["TaskTable"];
            GrdEdital.DataBind();
        }

        protected void GrdEdital_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = Session["TaskTable"] as DataTable;

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(ViewState["sortdr"]) == "Asc")
                {
                    dt.DefaultView.Sort = e.SortExpression + " Desc";
                    ViewState["sortdr"] = "Desc";
                }
                else
                {
                    dt.DefaultView.Sort = e.SortExpression + " Asc";
                    ViewState["sortdr"] = "Asc";
                }
                GrdEdital.DataSource = dt;
                GrdEdital.DataBind();
            }
        }

        protected void GrdEdital_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdEdital.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void GrdEdital_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName.CompareTo("Editar") == 0) || (e.CommandName.CompareTo("Deletar") == 0) || (e.CommandName.CompareTo("Aprovado") == 0))
            {
                if (GrdEdital.Columns.Count > 0)
                {
                    int id = (int)GrdEdital.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;

                    lblId.Text = id.ToString();

                    if (e.CommandName.CompareTo("Editar") == 0)
                    {
                        int index = Convert.ToInt32(e.CommandArgument);
                        GridViewRow row = GrdEdital.Rows[index];

                        foreach (TableCell cell in row.Cells)
                        {
                            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#75F94D"); //verde Liberado
                        }
                        txtObjeto.InnerText = Server.HtmlDecode(GrdEdital.Rows[index].Cells[6].Text);
                    }
                    else
                    {
                        if (e.CommandName.CompareTo("Deletar") == 0)
                        {
                            conexao.Open();

                            string Upd = "update tb_edital set DataExclusao=@exclusao where ID=@id";
                            MySqlCommand qryUpdate = new MySqlCommand(Upd, conexao);
                            qryUpdate = new MySqlCommand(Upd, conexao);
                            qryUpdate.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(lblId.Text);
                            qryUpdate.Parameters.Add("@exclusao", MySqlDbType.DateTime).Value = DateTime.Now;

                            try
                            {
                                qryUpdate.ExecuteNonQuery();

                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Processo Executado Com Sucesso.');", true);
                            }
                            catch
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Erro Para Updatar o Edital');", true);
                            }
                            finally
                            {
                                qryUpdate.Dispose();

                                conexao.Close();
                                Carrega();
                            }
                        }
                        else
                        {
                            if (e.CommandName.CompareTo("Aprovado") == 0)
                            {
                                conexao.Open();

                                string Upd = "update tb_edital set DataAprovacao=@aprovado where ID=@id";
                                MySqlCommand qryUpdate = new MySqlCommand(Upd, conexao);
                                qryUpdate = new MySqlCommand(Upd, conexao);
                                qryUpdate.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(lblId.Text);
                                qryUpdate.Parameters.Add("@aprovado", MySqlDbType.DateTime).Value = DateTime.Now;

                                try
                                {
                                    qryUpdate.ExecuteNonQuery();

                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Processo Executado Com Sucesso.');", true);
                                }
                                catch
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Erro Para Updatar o Aprovação');", true);
                                }
                                finally
                                {
                                    qryUpdate.Dispose();

                                    conexao.Close();
                                    Carrega();
                                }
                            }
                        }

                    }
                }
            }
        }

        private DataTable CriaDataTableN1()
        {

            DataTable DataTableN1 = new DataTable();
            DataColumn DataColumnN1;
            DataColumnN1 = new DataColumn();
            DataColumnN1.DataType = Type.GetType("System.String");
            DataColumnN1.ColumnName = "n1_id";
            DataTableN1.Columns.Add(DataColumnN1);

            DataColumnN1 = new DataColumn();
            DataColumnN1.DataType = Type.GetType("System.String");
            DataColumnN1.ColumnName = "n1_texto";
            DataTableN1.Columns.Add(DataColumnN1);

            DataColumnN1 = new DataColumn();
            DataColumnN1.DataType = Type.GetType("System.String");
            DataColumnN1.ColumnName = "n1_peso";
            DataTableN1.Columns.Add(DataColumnN1);

            return DataTableN1;
        }

        private DataTable CriaDataTableN2()
        {
            DataTable DataTableN2 = new DataTable();
            DataColumn DataColumnN2;
            DataColumnN2 = new DataColumn();
            DataColumnN2.DataType = Type.GetType("System.String");
            DataColumnN2.ColumnName = "n2_id";
            DataTableN2.Columns.Add(DataColumnN2);

            DataColumnN2 = new DataColumn();
            DataColumnN2.DataType = Type.GetType("System.String");
            DataColumnN2.ColumnName = "n2_texto";
            DataTableN2.Columns.Add(DataColumnN2);

            DataColumnN2 = new DataColumn();
            DataColumnN2.DataType = Type.GetType("System.String");
            DataColumnN2.ColumnName = "n2_peso";
            DataTableN2.Columns.Add(DataColumnN2);
            return DataTableN2;
        }

        private void incluirNoDataTable(DataTable Tabela, string texto, string peso, int nivel, string id)
        {
            DataRow linha;

            linha = Tabela.NewRow();

            if (nivel == 1)
            {
                linha["n1_id"] = id;
                linha["n1_texto"] = texto;
                linha["n1_peso"] = peso;
            }
            else
            {
                linha["n2_id"] = id;
                linha["n2_texto"] = texto;
                linha["n2_peso"] = peso;
            }

            Tabela.Rows.Add(linha);

            if (nivel == 1)
            {
                GrdNivel1.DataSource = ((DataTable)Tabela).DefaultView;

                GrdNivel1.DataBind();
            }
            else
            {
                GrdNivel2.DataSource = ((DataTable)Tabela).DefaultView;

                GrdNivel2.DataBind();
            }
        }

        protected void BtnGerar_Click(object sender, ImageClickEventArgs e)
        {
            if (txtObjeto.InnerText != "")
            {
                ddlClassificacao.SelectedValue = "";
                DataTable dataN1 = (DataTable)Session["DataTableN1"];
                dataN1.Clear();
                DataTable dataN2 = (DataTable)Session["DataTableN2"];
                dataN2.Clear();
                GrdNivel1.DataSource = null;
                GrdNivel1.DataBind();
                GrdNivel2.DataSource = null;
                GrdNivel2.DataBind();

                string objeto = txtObjeto.InnerText;

                if (objeto.Substring(0, 1) == "[")
                {
                    objeto = objeto.Substring(objeto.IndexOf("]") + 1, objeto.Length - (objeto.IndexOf("]") + 1)).TrimStart();
                }

                if (objeto.Substring(0, 3) == "* L")
                {
                    objeto = objeto.Substring(2, objeto.Length - 2);
                    objeto = objeto.Substring(objeto.IndexOf("*") + 1, objeto.Length - (objeto.IndexOf("*") + 1)).TrimStart();
                }

                string[] palavras = objeto.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', '"' }, StringSplitOptions.RemoveEmptyEntries);

                int conta = palavras.Count();

                int nivel_1 = 0;
                int nivel_2 = 0;
                string texto_1 = "";
                string texto_2 = "";
                int n1 = 0;
                int n2 = 0;
                string palavran = "";
                int peso = 0;

                foreach (string palavra in palavras)
                {
                    string pala = palavra.Replace("'", "");
                    pala = pala.Trim().ToUpper();

                    if (pala.Length > 0)
                    {
                        if (pala.Length > 1)
                        {
                            if (pala.Substring(pala.Length - 1, 1) == "," || pala.Substring(pala.Length - 1, 1) == "." || pala.Substring(pala.Length - 1, 1) == ")" || pala.Substring(pala.Length - 1, 1) == "}" || pala.Substring(pala.Length - 1, 1) == "]" || pala.Substring(pala.Length - 1, 1) == ";" || pala.Substring(pala.Length - 1, 1) == ":")
                            {
                                palavran = pala.Substring(0, pala.Length - 1);

                                if (palavran.Substring(palavran.Length - 1, 1) == "A")
                                    palavran = palavran.Substring(0, palavran.Length - 1);

                                if (palavran.Substring(palavran.Length - 1, 1) == "S")
                                {
                                    palavran = palavran.Substring(0, palavran.Length - 1);
                                    if (palavran.Substring(palavran.Length - 1, 1) == "A")
                                        palavran = palavran.Substring(0, palavran.Length - 1);
                                }
                            }
                            else
                            {
                                palavran = pala;
                            }

                            if (palavran.Length > 1)
                            {
                                if (palavran.Substring(0, 1) == "(" || palavran.Substring(0, 1) == "," || palavran.Substring(0, 1) == "." || palavran.Substring(0, 1) == "{" || palavran.Substring(0, 1) == "[" || palavran.Substring(0, 1) == ";" || palavran.Substring(0, 1) == ":")
                                    palavran = palavran.Substring(1, palavran.Length - 1);
                            }
                        }

                        if (palavran.Length > 3 && (palavran != "PARA"))
                        {
                            conexao.Open();

                            string SelN1 = "Select ID,Peso,Valor from tb_palavra where Valor like('" + palavran + "%') and Nivel=1 and Inativo=0";
                            MySqlCommand qrySelectN1 = new MySqlCommand(SelN1, conexao);
                            qrySelectN1 = new MySqlCommand(SelN1, conexao);

                            MySqlDataReader readerN1 = qrySelectN1.ExecuteReader();

                            while (readerN1.Read())
                            {
                                nivel_1 = Convert.ToInt32(readerN1["ID"].ToString());
                                texto_1 = readerN1["Valor"].ToString();
                                peso = Convert.ToInt32(readerN1["Peso"].ToString());
                                n1++;

                                incluirNoDataTable((DataTable)Session["DataTableN1"], readerN1["Valor"].ToString(), readerN1["Peso"].ToString(), 1, readerN1["ID"].ToString());
                            }

                            qrySelectN1.Dispose();

                            conexao.Close();

                            conexao.Open();

                            string SelN2 = "Select ID,Peso,Valor from tb_palavra where Valor like('" + palavran + "%') and Nivel=2 and Inativo=0";
                            MySqlCommand qrySelectN2 = new MySqlCommand(SelN2, conexao);
                            qrySelectN2 = new MySqlCommand(SelN2, conexao);

                            MySqlDataReader readerN2 = qrySelectN2.ExecuteReader();

                            while (readerN2.Read())
                            {
                                nivel_2 = Convert.ToInt32(readerN2["ID"].ToString());
                                texto_2 = readerN2["Valor"].ToString();
                                peso = Convert.ToInt32(readerN2["Peso"].ToString());
                                n2++;

                                incluirNoDataTable((DataTable)Session["DataTableN2"], readerN2["Valor"].ToString(), readerN2["Peso"].ToString(), 2, readerN2["ID"].ToString());
                            }

                            qrySelectN2.Dispose();

                            conexao.Close();
                        }
                    }
                }

                if (n1 > 0 && n2 > 0)
                {
                    Classifica();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('O Texto do Objeto Deve Ser Digitado');", true);
                txtObjeto.Focus();
            }
        }

        public void Classifica()
        {
            DataTable dataN1 = (DataTable)Session["DataTableN1"];
            dataN1.DefaultView.Sort = "n1_peso Desc";
            dataN1 = dataN1.DefaultView.ToTable();
            GrdNivel1.DataSource = dataN1;
            GrdNivel1.DataBind();

            DataTable dataN2 = (DataTable)Session["DataTableN2"];
            dataN2.DefaultView.Sort = "n2_peso Desc";
            dataN2 = dataN2.DefaultView.ToTable();
            GrdNivel2.DataSource = dataN2;
            GrdNivel2.DataBind();

            int nivel_1 = 0;
            int nivel_2 = 0;
            string texto_1 = "";
            string texto_2 = "";

            foreach (DataRow RowN1 in dataN1.Rows)
            {
                nivel_1 = Convert.ToInt32(RowN1["n1_id"]);
                texto_1 = RowN1["n1_texto"].ToString();

                foreach (DataRow RowN2 in dataN2.Rows)
                {
                    nivel_2 = Convert.ToInt32(RowN2["n2_id"]);
                    texto_2 = RowN2["n2_texto"].ToString();

                    conexao.Open();

                    string SelC = "select b.ID,b.Descricao from rl_classificacao_palavra as a inner join tb_classificacao as b on a.Classificacao=b.ID where a.Nivel1=@nivel1 and a.Nivel2=@nivel2 and a.Inativo=0 and b.Inativo=0";
                    MySqlCommand qrySelectC = new MySqlCommand(SelC, conexao);
                    qrySelectC = new MySqlCommand(SelC, conexao);
                    qrySelectC.Parameters.Add("@nivel1", MySqlDbType.Int32).Value = nivel_1;
                    qrySelectC.Parameters.Add("@nivel2", MySqlDbType.Int32).Value = nivel_2;

                    MySqlDataReader readerC = qrySelectC.ExecuteReader();
                    lblClassificacao.Text = "";

                    while (readerC.Read())
                    {
                        lblClassificacao.Text = readerC["Descricao"].ToString() + "    Classificou Nível-1: " + texto_1 + "   -   Nível-2 : " + texto_2;
                        ddlClassificacao.SelectedValue = readerC["ID"].ToString();
                    }

                    qrySelectC.Dispose();

                    conexao.Close();

                    if (lblClassificacao.Text != "")
                        break;
                }

                if (lblClassificacao.Text == "")
                {
                    lblClassificacao.Text = "Não Conseguiu Classificar Com Nenhuma Palavra de Nível-1 e Nível-2 das Relações";
                }
                else
                {
                    BtnGrava.Visible = true;
                    break;
                }
            }
        }

        protected void BtnGrava_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlClassificacao.SelectedValue != "" && lblId.Text != "")
            {
                conexao.Open();

                string Upd = "update tb_edital set ID_Classificacao=@classificacao,DataEdicao=@edicao where ID=@id";
                MySqlCommand qryUpdate = new MySqlCommand(Upd, conexao);
                qryUpdate = new MySqlCommand(Upd, conexao);
                qryUpdate.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(lblId.Text);
                qryUpdate.Parameters.Add("@classificacao", MySqlDbType.Int16).Value = Convert.ToInt32(ddlClassificacao.SelectedValue);
                qryUpdate.Parameters.Add("@edicao", MySqlDbType.DateTime).Value = DateTime.Now;
                try
                {
                    qryUpdate.ExecuteNonQuery();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Processo Executado Com Sucesso.');", true);
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Erro na Update de Edital na GRavação de Classificação');", true);
                }
                finally
                {
                    qryUpdate.Dispose();

                    conexao.Close();
                }
                ddlClassificacao.SelectedValue = "";
                txtObjeto.InnerText = "";
                GrdEdital.SelectedIndex = -1;
                rdbSim.Checked = false;
                rdbNao.Checked = true;
                lblId.Text = "";
                Carrega();
                BtnGrava.Visible = false;
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('A Classificação Deve Ser Escolhidal');", true);

        }

        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            if (TxtDataIni.Text != "" && TxtDataFim.Text != "")
                Carrega();
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('As Datas Para o Período de Busca de Editais Devem ser Digitadas');", true);
        }

        protected void ChbDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (ChbDelete.Checked == true)
            {
                GrdEdital.Enabled = false;
                ChbEditados.Checked = false;
                ChbAprovado.Checked = false;
            }
            else
                GrdEdital.Enabled = true;
        }


        protected void ChbEditados_CheckedChanged(object sender, EventArgs e)
        {
            if (ChbEditados.Checked == true)
            {
                ChbDelete.Checked = false;
                ChbAprovado.Checked = false;
            }
        }

        protected void ChbAprovado_CheckedChanged(object sender, EventArgs e)
        {
            if (ChbAprovado.Checked == true)
            {
                ChbDelete.Checked = false;
                ChbEditados.Checked = false;
            }
        }

        protected void ddlClassificacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlClassificacao.SelectedValue != "" && lblId.Text != "")
                BtnGrava.Visible = true;
        }
    }
}