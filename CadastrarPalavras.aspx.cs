using AjaxControlToolkit;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Classificacao
{
    public partial class CadastrarPalavras : System.Web.UI.Page
    {
        Boolean erro = false;

        MySqlConnection conexao = new MySqlConnection(String.Format(ConfigurationManager.AppSettings["StrDbEdital"]));

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                TabContainer1.ActiveTabIndex = 0;
                Carrega(0);
            }

        }

        public void Carrega(int opcao)
        {
            conexao.Open();

            string Sel = "SELECT pl_id,pl_palavra from tb_palavras_licitacao ";

            if (opcao == 1)
                Sel = Sel + " where pl_palavra like '" + txtPalavra.Text + "%'";

            if (opcao == 2)
                Sel = Sel + " where pl_palavra like '%" + txtPalavra.Text + "%'";

            if (opcao == 3)
                Sel = Sel + " where pl_palavra like '%" + txtPalavra.Text + "'";

            Sel = Sel + " order by pl_palavra";

            MySqlCommand qrySelect = new MySqlCommand(Sel, conexao);
            DataTable dataTable = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(qrySelect);
            da.Fill(dataTable);

            Session["TaskTable"] = dataTable;

            GrdPalavra.DataSource = dataTable;
            GrdPalavra.DataBind();
            ViewState["dirState"] = dataTable;
            ViewState["sortdr"] = "Asc";

            qrySelect.Dispose();

            conexao.Close();
        }
        public void Consiste()
        {
            if (txtPalavra.Text == "")
            {
                erro = true;

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('A Palavras Filtro de Licitações Deve Ser Digitada.');", true);
            }
        }
        protected void BtnGrava_Click(object sender, ImageClickEventArgs e)
        {
            Consiste();

            if (erro == false)
            {
                if (lblId.Text == "")
                {
                    conexao.Open();

                    string SelC = "Select count(*) as qtde from tb_palavras_licitacao where pl_palavra=@nome";
                    MySqlCommand qrySelectC = new MySqlCommand(SelC, conexao);
                    qrySelectC = new MySqlCommand(SelC, conexao);
                    qrySelectC.Parameters.Add("@nome", MySqlDbType.VarChar, 40).Value = txtPalavra.Text.ToUpper();

                    MySqlDataReader readerC = qrySelectC.ExecuteReader();
                    int qtde = 0;
                    while (readerC.Read())
                    {
                        qtde = Convert.ToInt32(readerC["qtde"].ToString());
                    }

                    qrySelectC.Dispose();

                    conexao.Close();

                    if (qtde == 0)
                    {
                        conexao.Open();

                        string Ins = "insert INTO tb_palavras_licitacao(pl_palavra) values(@nome)";
                        MySqlCommand qryInsert = new MySqlCommand(Ins, conexao);
                        qryInsert = new MySqlCommand(Ins, conexao);
                        qryInsert.Parameters.Add("@nome", MySqlDbType.VarChar, 40).Value = txtPalavra.Text.ToUpper();

                        try
                        {
                            qryInsert.ExecuteNonQuery();
                        }
                        catch
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Erro na Insert de Palavras Filtro de Licitações.');", true);

                            erro = true;
                        }
                        finally
                        {
                            qryInsert.Dispose();

                            conexao.Close();
                        }

                        if (erro == false)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Processo Executado Com Sucesso.');", true);
                        }
                    }
                    else
                    {
                        erro = true;

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Já Existe a Palavra Filtro de Licitações !');", true);
                    }
                }
                else
                {
                    conexao.Open();

                    string Upd = "update tb_palavras_licitacao set pl_palavra=@nome where pl_id=@id";
                    MySqlCommand qryUpdate = new MySqlCommand(Upd, conexao);
                    qryUpdate = new MySqlCommand(Upd, conexao);
                    qryUpdate.Parameters.Add("@id", MySqlDbType.Int16).Value = Convert.ToInt16(lblId.Text);
                    qryUpdate.Parameters.Add("@nome", MySqlDbType.VarChar, 30).Value = txtPalavra.Text.ToUpper();

                    try
                    {
                        qryUpdate.ExecuteNonQuery();
                    }
                    catch
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Erro na Update de Palavras Filtro de Licitações.');", true);

                        erro = true;
                    }
                    finally
                    {
                        qryUpdate.Dispose();

                        conexao.Close();
                    }

                    if (erro == false)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Processo Executado Com Sucesso.');", true);
                    }
                }
            }

            GrdPalavra.SelectedIndex = -1;
            Carrega(0);
            lblId.Text = "";
            Limpa();
        }
        protected void BtnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            conexao.Open();

            string Del = "delete from tb_palavras_licitacao where pl_id=@id";
            MySqlCommand qryDelete = new MySqlCommand(Del, conexao);
            qryDelete = new MySqlCommand(Del, conexao);
            qryDelete.Parameters.Add("@id", MySqlDbType.Int16).Value = Convert.ToInt16(lblId.Text);

            try
            {
                qryDelete.ExecuteNonQuery();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Erro na Delete de Palavras Filtro de Licitações.');", true);

                erro = true;
            }
            finally
            {
                qryDelete.Dispose();

                conexao.Close();
            }

            if (erro == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Processo Executado Com Sucesso.');", true);
            }

            GrdPalavra.SelectedIndex = -1;
            Carrega(0);
            lblId.Text = "";
            Limpa();
        }
        protected void BtnLimpa_Click(object sender, ImageClickEventArgs e)
        {
            Limpa();
            GrdPalavra.SelectedIndex = -1;
            Carrega(0);
        }
        public void Limpa()
        {
            lblId.Text = "";
            txtPalavra.Text = "";
        }
        protected void GrdPalavra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GrdPalavra.Columns.Count > 0)
            {
                GridViewRow row = GrdPalavra.SelectedRow;
                lblId.Text = GrdPalavra.DataKeys[row.RowIndex].Values["pl_id"].ToString();
                LblPalavra.Text = Server.HtmlDecode(GrdPalavra.SelectedRow.Cells[1].Text);
                txtPalavra.Text = Server.HtmlDecode(GrdPalavra.SelectedRow.Cells[1].Text);
                CarregaEx(Convert.ToInt16(lblId.Text));
                txtPalavra.Focus();
            }
        }
        protected void GrdPalavra_Sorting(object sender, GridViewSortEventArgs e)
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
                GrdPalavra.DataSource = dt;
                GrdPalavra.DataBind();
            }
        }

        protected void GrdPalavra_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdPalavra.PageIndex = e.NewPageIndex;
            BindData();
        }
        private void BindData()
        {
            GrdPalavra.DataSource = Session["TaskTable"];
            GrdPalavra.DataBind();
        }
        protected void btnInicio_Click(object sender, EventArgs e)
        {
            if (txtPalavra.Text != "")
                Carrega(1);
        }

        protected void btnContem_Click(object sender, EventArgs e)
        {
            if (txtPalavra.Text != "")
                Carrega(2);
        }

        protected void btnFim_Click(object sender, EventArgs e)
        {
            if (txtPalavra.Text != "")
                Carrega(3);
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            if (txtPalavra.Text != "")
                Carrega(1);
        }

        // Excludentes 

        public void CarregaEx(int opcao)
        {
            conexao.Open();

            string Sel = "SELECT pe_id,pe_excludente from tb_palavras_excludente where pe_palavra=@palavra ";

            if (opcao == 1)
                Sel = Sel + " where pe_excludente like '" + txtExcludente.Text + "%'";

            if (opcao == 2)
                Sel = Sel + " where pe_excludente like '%" + txtExcludente.Text + "%'";

            if (opcao == 3)
                Sel = Sel + " where pe_excludente like '%" + txtExcludente.Text + "'";

            Sel = Sel + " order by pe_excludente";

            MySqlCommand qrySelect = new MySqlCommand(Sel, conexao);
            qrySelect.Parameters.Add("@palavra", MySqlDbType.Int16).Value = Convert.ToInt16(lblId.Text);
            DataTable dataTable = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(qrySelect);
            da.Fill(dataTable);

            Session["TaskTableEx"] = dataTable;

            GrdExcludente.DataSource = dataTable;
            GrdExcludente.DataBind();
            ViewState["dirState"] = dataTable;
            ViewState["sortdr"] = "Asc";

            qrySelect.Dispose();

            conexao.Close();

            LimpaEx();
        }

        public void ConsisteC()
        {
            if (txtPalavra.Text == "")
            {
                erro = true;

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('A Palavras Filtro de Licitações Deve Ser Digitada.');", true);
            }
        }
        protected void BtnGravaEx_Click(object sender, ImageClickEventArgs e)
        {
            ConsisteC();

            if (erro == false)
            {
                if (lblIdEx.Text == "")
                {
                    conexao.Open();

                    string SelCEx = "Select count(*) as qtde from tb_palavras_excludente where pe_excludente=@nome and pe_palavra=@palavra";
                    MySqlCommand qrySelectCEx = new MySqlCommand(SelCEx, conexao);
                    qrySelectCEx = new MySqlCommand(SelCEx, conexao);
                    qrySelectCEx.Parameters.Add("@nome", MySqlDbType.VarChar, 40).Value = txtExcludente.Text.ToUpper();
                    qrySelectCEx.Parameters.Add("@palavra", MySqlDbType.Int16).Value = Convert.ToInt16(lblId.Text);
                    MySqlDataReader readerCEx = qrySelectCEx.ExecuteReader();
                    int qtde = 0;
                    while (readerCEx.Read())
                    {
                        qtde = Convert.ToInt32(readerCEx["qtde"].ToString());
                    }

                    qrySelectCEx.Dispose();

                    conexao.Close();

                    if (qtde == 0)
                    {
                        conexao.Open();

                        string InsEx = "insert INTO tb_palavras_excludente(pe_palavra,pe_excludente) values(@palavra,@excludente)";
                        MySqlCommand qryInsertEx = new MySqlCommand(InsEx, conexao);
                        qryInsertEx = new MySqlCommand(InsEx, conexao);
                        qryInsertEx.Parameters.Add("@palavra", MySqlDbType.Int16).Value = Convert.ToInt16(lblId.Text);
                        qryInsertEx.Parameters.Add("@excludente", MySqlDbType.VarChar, 40).Value = txtExcludente.Text.ToUpper();

                        try
                        {
                            qryInsertEx.ExecuteNonQuery();
                        }
                        catch
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Erro na Insert de Palavras Excludentes Filtro de Licitações.');", true);

                            erro = true;
                        }
                        finally
                        {
                            qryInsertEx.Dispose();

                            conexao.Close();
                        }

                        if (erro == false)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Processo Executado Com Sucesso.');", true);
                        }
                    }
                    else
                    {
                        erro = true;

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Já Existe a Palavra Filtro de Licitações !');", true);
                    }
                }
                else
                {
                    conexao.Open();

                    string UpdEx = "update tb_palavras_excludente set pe_excludente=@excludente where pe_id=@id";
                    MySqlCommand qryUpdateEx = new MySqlCommand(UpdEx, conexao);
                    qryUpdateEx = new MySqlCommand(UpdEx, conexao);
                    qryUpdateEx.Parameters.Add("@id", MySqlDbType.Int16).Value = Convert.ToInt16(lblIdEx.Text);
                    qryUpdateEx.Parameters.Add("@excludente", MySqlDbType.VarChar, 30).Value = txtExcludente.Text.ToUpper();

                    try
                    {
                        qryUpdateEx.ExecuteNonQuery();
                    }
                    catch
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Erro na Update de Palavras Excludente Filtro de Licitações.');", true);

                        erro = true;
                    }
                    finally
                    {
                        qryUpdateEx.Dispose();

                        conexao.Close();
                    }

                    if (erro == false)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Processo Executado Com Sucesso.');", true);
                    }
                }
            }

            GrdExcludente.SelectedIndex = -1;
            CarregaEx(0);
            lblIdEx.Text = "";
            LimpaEx();
        }

        protected void BtnExcluirEx_Click(object sender, ImageClickEventArgs e)
        {
            conexao.Open();

            string DelEx = "delete from tb_palavras_excludente where pe_id=@id";
            MySqlCommand qryDeleteEx = new MySqlCommand(DelEx, conexao);
            qryDeleteEx = new MySqlCommand(DelEx, conexao);
            qryDeleteEx.Parameters.Add("@id", MySqlDbType.Int16).Value = Convert.ToInt16(lblIdEx.Text);

            try
            {
                qryDeleteEx.ExecuteNonQuery();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Erro na Delete de Palavras Excludente Filtro de Licitações .');", true);

                erro = true;
            }
            finally
            {
                qryDeleteEx.Dispose();

                conexao.Close();
            }

            if (erro == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Processo Executado Com Sucesso.');", true);
            }

            GrdExcludente.SelectedIndex = -1;
            CarregaEx(0);
            lblIdEx.Text = "";
            LimpaEx();
        }

        protected void BtnLimpaEx_Click(object sender, ImageClickEventArgs e)
        {
            LimpaEx();
            GrdExcludente.SelectedIndex = -1;
            Carrega(0);
        }

        public void LimpaEx()
        {
            lblIdEx.Text = "";
            txtExcludente.Text = "";
        }
        protected void GrdExcludente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GrdExcludente.Columns.Count > 0)
            {
                GridViewRow row = GrdExcludente.SelectedRow;
                lblIdEx.Text = GrdExcludente.DataKeys[row.RowIndex].Values["pe_id"].ToString();
                txtExcludente.Text = Server.HtmlDecode(GrdExcludente.SelectedRow.Cells[1].Text);
                txtExcludente.Focus();
            }
        }

        protected void GrdExcludente_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = Session["TaskTableEx"] as DataTable;

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
                GrdExcludente.DataSource = dt;
                GrdExcludente.DataBind();
            }
        }
        protected void GrdExcludente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdExcludente.PageIndex = e.NewPageIndex;
            BindDataEx();
        }

        private void BindDataEx()
        {
            GrdExcludente.DataSource = Session["TaskTableEx"];
            GrdExcludente.DataBind();
        }

        protected void btnFimEx_Click(object sender, EventArgs e)
        {
            if (txtExcludente.Text != "")
                CarregaEx(3);
        }

        protected void btnIniEx_Click(object sender, EventArgs e)
        {
            if (txtExcludente.Text != "")
                CarregaEx(1);
        }

        protected void btnConEx_Click(object sender, EventArgs e)
        {
            if (txtExcludente.Text != "")
                CarregaEx(2);
        }
    }
}