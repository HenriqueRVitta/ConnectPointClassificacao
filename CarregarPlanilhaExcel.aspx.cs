using ExcelDataReader;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Classificacao
{
    public partial class CarregarPlanilhaExcel : System.Web.UI.Page
    {

        MySqlConnection conexao = new MySqlConnection(String.Format(ConfigurationManager.AppSettings["StrDbEdital"]));

        internal DataTable dtbN1 = null;
        internal DataTable dtbN2 = null;
        Boolean erro = false;


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnCarregar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string path = "./carga/";
            
            if (!Directory.Exists(path))
                Directory.CreateDirectory(Server.MapPath(path));

            try
            {
                if (FileUpload1.HasFile)
                {
                    foreach (var file in FileUpload1.PostedFiles)
                    {
                        FileUpload1.SaveAs(Server.MapPath(path) + System.IO.Path.GetFileName("carga.xlsx"));
                    }

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Arquivos Carregados Com Sucesso !');", true);
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Arquivo não Foram Carregados !');", true);
            }

        }
        protected void BtnGravar_Click(object sender, ImageClickEventArgs e)
        {
            int modalidade = 0;
            //campos[0] = Número do Licitacao   
            //campos[1] = Código              
            //campos[2] = Orgão  
            //campos[3] = Endereço             
            //campos[4] = Cidade   
            //campos[5] = Estado           
            //campos[6] = CEP        
            //campos[7] = Edital    
            //campos[8] = Site I              
            //campos[9] = Site II           
            //campos[10] = Processo             
            //campos[11] = Valor Estimado                
            //campos[12] = Itens        
            //campos[13] = Situação        
            //campos[14] = Documento                
            //campos[15] = Abertura   
            //campos[16] = Abertura      
            //campos[17] = Prazo             
            //campos[18] = Objeto          
            //campos[19] = Observação      
            //campos[20] = Anexos        
            //campos[21] = Atualizada em

            string filename = @"./carga/carga.xlsx";
            string arquivo = Server.MapPath(@filename);

            int registro = 0;
            int conta = 0;
            using (var stream = System.IO.File.Open(@arquivo, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    if (result != null)
                    {
                        var table = result.Tables[0];

                        if (table != null)
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                registro++;

                                var campos = row.ItemArray;

                                if (campos.Count() == 23 && registro > 1)
                                {
                                    if (campos[7].ToString().Substring(0, 2) == "SM")
                                        modalidade = 1;
                                    if (campos[7].ToString().Substring(0, 2) == "CV")
                                        modalidade = 2;
                                    if (campos[7].ToString().Substring(0, 2) == "CR")
                                        modalidade = 3;
                                    if (campos[7].ToString().Substring(0, 2) == "LE")
                                        modalidade = 4;
                                    if (campos[7].ToString().Substring(0, 2) == "TP")
                                        modalidade = 5;
                                    if (campos[7].ToString().Substring(0, 2) == "PE")
                                        modalidade = 6;
                                    if (campos[7].ToString().Substring(0, 2) == "DL")
                                        modalidade = 7;
                                    if (campos[7].ToString().Substring(0, 2) == "PR")
                                        modalidade = 9;
                                    if (campos[7].ToString().Substring(0, 3) == "RDC")
                                        modalidade = 15;
                                    if (campos[7].ToString().Substring(0, 2) == "CE")
                                        modalidade = 13;
                                    if (campos[7].ToString().Substring(0, 2) == "CP")
                                        modalidade = 20;
                                    if (campos[7].ToString().Substring(0, 2) == "PE")
                                        modalidade = 6;

                                    string objeto = campos[18].ToString();

                                    if (objeto.Substring(0, 1) == "[")
                                    {
                                        objeto = objeto.Substring(objeto.IndexOf("]") + 1, objeto.Length - (objeto.IndexOf("]") + 1)).TrimStart();
                                    }

                                    if (objeto.Substring(0, 3) == "* L")
                                    {
                                        objeto = objeto.Substring(2, objeto.Length - 2);
                                        objeto = objeto.Substring(objeto.IndexOf("*") + 1, objeto.Length - (objeto.IndexOf("*") + 1)).TrimStart();
                                    }

                                    int Estado = 0;
                                    int Cidade = 0;

                                    conexao.Open();

                                    string SelE = "select ID from tp_estados where Sigla=@sigla";
                                    MySqlCommand qrySelectE = new MySqlCommand(SelE, conexao);
                                    qrySelectE.Parameters.Add("@sigla", MySqlDbType.VarChar, 3).Value = campos[5].ToString();

                                    MySqlDataReader readerE = qrySelectE.ExecuteReader();

                                    while (readerE.Read())
                                    {
                                        Estado = Convert.ToInt32(readerE["ID"].ToString());
                                    }

                                    qrySelectE.Dispose();

                                    conexao.Close();

                                    conexao.Open();

                                    string SelCi = "select ID from tp_cidades where Nome=@nome and ID_Estado=@estado";
                                    MySqlCommand qrySelectCi = new MySqlCommand(SelCi, conexao);
                                    qrySelectCi.Parameters.Add("@nome", MySqlDbType.VarChar).Value = campos[4].ToString();
                                    qrySelectCi.Parameters.Add("@estado", MySqlDbType.Int32).Value = Estado;

                                    MySqlDataReader readerCi = qrySelectCi.ExecuteReader();

                                    while (readerCi.Read())
                                    {
                                        Cidade = Convert.ToInt32(readerCi["ID"].ToString());
                                    }

                                    qrySelectCi.Dispose();

                                    conexao.Close();

                                    /*  Verifico se ja foi ja exite o registro
                                     *  caso existir ignora 
                                     */
                                    conexao.Open();
                                    var codigoLicitacao = "";
                                    string codLicita = campos[7].ToString();
                                    string SelRepet = "select id_cidade,id_estado,codigolicitacao from tb_edital where id_cidade=@Cidade and id_estado=@Estado and codigolicitacao = @codLicita";
                                    MySqlCommand qrySelectRepet = new MySqlCommand(SelRepet, conexao);
                                    qrySelectRepet.Parameters.Add("@Cidade", MySqlDbType.Int32).Value = Cidade;
                                    qrySelectRepet.Parameters.Add("@Estado", MySqlDbType.Int32).Value = Estado;
                                    qrySelectRepet.Parameters.Add("@codLicita", MySqlDbType.VarChar, 255).Value = campos[7].ToString();
                                    MySqlDataReader readerRepet = qrySelectRepet.ExecuteReader();

                                    while (readerRepet.Read())
                                    {
                                        codigoLicitacao = readerRepet["codigolicitacao"].ToString();
                                    }
                                    qrySelectE.Dispose();
                                    conexao.Close();

                                    /* Se ja existir no banco passa para o próximo registro */
                                    if (codigoLicitacao.Length > 0)
                                        continue;


                                    DateTime data;
                                    string Data_Abertura = "";

                                    if (campos[15].ToString() != "Não informado")
                                    {
                                        try
                                        {
                                            data = Convert.ToDateTime(campos[15].ToString() + " " + campos[16].ToString());
                                            Data_Abertura = data.ToString();
                                        }
                                        catch
                                        {
                                            Data_Abertura = "";
                                        }
                                    }
                                    else
                                    {
                                        if (campos[14].ToString() != "Não informado")
                                        {
                                            try
                                            {
                                                data = Convert.ToDateTime(campos[14].ToString());
                                                Data_Abertura = data.ToString();
                                            }
                                            catch
                                            {
                                                Data_Abertura = "";


                                            }
                                        }
                                        else
                                        if (campos[17].ToString() != "Não informado")
                                        {
                                            try
                                            {
                                                data = Convert.ToDateTime(campos[17].ToString());
                                                Data_Abertura = data.ToString();
                                            }
                                            catch
                                            {
                                                Data_Abertura = "";
                                            }
                                        }
                                        else
                                            Data_Abertura = "";
                                    }

                                    string valor = campos[11].ToString();

                                    if (campos[11].ToString().Length > 0)
                                        valor = campos[11].ToString().Replace(",", ".");

                                    conexao.Open();

                                    string Ins = "insert ignore into tb_edital(ID_Cidade,ID_Estado,CaminhoAnexo,CodigoLicitacao,DataAbertura,DataPublicacao,EmailContato,FonteLicitacao,Modalidade,Nome,Objeto,Observacoes,OrgaoResponsavel,TelefoneContato,TipoLocalizacao,ValorLicitacao,ValorSigiloso,DataCadastro,DataEdicao,DataExclusao) values(@Cidade,@Estado,@Anexo,@Codigo,@Abertura,@Publicacao,@Email,@Licitacao,@Modalidade,@Nome,@Objeto,@Obs,@Responsavel,@Contato,@Localizacao,@Licitado,@Sigiloso,@Cadastro,@Edicao,@Exclusao)";
                                    MySqlCommand qryInsert = new MySqlCommand(Ins, conexao);
                                    qryInsert.Parameters.Add("@Cidade", MySqlDbType.Int32).Value = Cidade;
                                    qryInsert.Parameters.Add("@Estado", MySqlDbType.Int32).Value = Estado;
                                    qryInsert.Parameters.Add("@Anexo", MySqlDbType.VarChar, 2083).Value = campos[20].ToString();
                                    qryInsert.Parameters.Add("@Codigo", MySqlDbType.VarChar, 255).Value = campos[7].ToString();

                                    if (Data_Abertura != "")
                                        qryInsert.Parameters.Add("@Abertura", MySqlDbType.DateTime).Value = Convert.ToDateTime(Data_Abertura);
                                    else
                                        qryInsert.Parameters.Add("@Abertura", MySqlDbType.DateTime).Value = null;

                                    qryInsert.Parameters.Add("@Publicacao", MySqlDbType.DateTime).Value = null;

                                    qryInsert.Parameters.Add("@Email", MySqlDbType.VarChar, 255).Value = null;
                                    if (campos[8].ToString().Length > 0)
                                        qryInsert.Parameters.Add("@Licitacao", MySqlDbType.VarChar, 2083).Value = campos[8].ToString();
                                    else
                                        if (campos[9].ToString().Length > 0)
                                        qryInsert.Parameters.Add("@Licitacao", MySqlDbType.VarChar, 2083).Value = campos[8].ToString();
                                    else
                                        qryInsert.Parameters.Add("@Licitacao", MySqlDbType.VarChar, 2083).Value = null;
                                    qryInsert.Parameters.Add("@Modalidade", MySqlDbType.Int32).Value = Convert.ToInt32(modalidade);
                                    qryInsert.Parameters.Add("@Nome", MySqlDbType.VarChar, 255).Value = campos[10].ToString();
                                    qryInsert.Parameters.Add("@Objeto", MySqlDbType.Text).Value = objeto;
                                    qryInsert.Parameters.Add("@Obs", MySqlDbType.Text).Value = campos[19].ToString();
                                    qryInsert.Parameters.Add("@Responsavel", MySqlDbType.VarChar, 255).Value = campos[2].ToString();
                                    qryInsert.Parameters.Add("@Contato", MySqlDbType.VarChar, 20).Value = null;
                                    qryInsert.Parameters.Add("@Localizacao", MySqlDbType.Int32).Value = null;
                                    if (valor != "")
                                        qryInsert.Parameters.Add("@Licitado", MySqlDbType.Decimal).Value = Convert.ToDecimal(valor);
                                    else
                                        qryInsert.Parameters.Add("@Licitado", MySqlDbType.Decimal).Value = null;
                                    qryInsert.Parameters.Add("@Sigiloso", MySqlDbType.Bit, 1).Value = 1;
                                    qryInsert.Parameters.Add("@Cadastro", MySqlDbType.DateTime).Value = DateTime.Now;
                                    qryInsert.Parameters.Add("@Edicao", MySqlDbType.DateTime).Value = null;
                                    qryInsert.Parameters.Add("@Exclusao", MySqlDbType.DateTime).Value = null;

                                    try
                                    {
                                        qryInsert.ExecuteNonQuery();
                                        conta++;
                                    }
                                    catch
                                    {
                                        //                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Erro no Insert no Banco da Connectpoint. Número Conlicitação : "+campos[0].ToString()+"');", true);
                                    }
                                    finally
                                    {
                                        qryInsert.Dispose();

                                        conexao.Close();
                                    }
                                }
                            }

                            if (erro == false)
                              TotalContador.Text = conta.ToString() + " - Registros Gravados";

                            if (erro == false)
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Processamento Executado Com Sucesso." + conta.ToString() + " - Registros Gravados');", true);
                            else
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensagem", "alert('Processamento Não Foi Executado. Problemas');", true);
                        }
                    }
                }
            }
        }

    }
}