<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClassificaTexto.aspx.cs" Inherits="Classificacao.ClassificaTexto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            <asp:Panel ID="PnlConnect" runat="server">
            <asp:UpdatePanel ID="UpdConnect" runat="server">
                <ContentTemplate>

             <table align="center">
                <tr>
                    <td colspan="4" align="center">
                        <h2 style="color:#3431FF">Classificação de Texto</h2>
                        <asp:Label ID="lblId" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                         &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="color:#3431FF" colspan="4" align="center">Texto a Ser Classificado</td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <textarea id="txtObjeto" name="Objeto" rows="5" with="950" title="Objeto da Licitação" runat="server" required="required"></textarea>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                         &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <table>
                            <tr>
                                <td title="Pressione o Botão Para Classificar o Texto">
                                    <asp:ImageButton ID="BtnGerar" runat="server" ImageUrl="imagens/gerar.png" OnClick="BtnGerar_Click"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                         &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdNivel1" runat="server" AutoGenerateColumns="False" DataKeyNames="n1_id,n1_texto,n1_peso" BackColor="White" BorderStyle="None" CellPadding="4" ForeColor="Black" GridLines="Vertical" Caption="Palvras de Nível I" AllowSorting="true" >
                                        <Columns>
                                            <asp:BoundField DataField="n1_id" HeaderText="ID" />
                                            <asp:BoundField DataField="n1_texto" HeaderText="Palavra" />
                                            <asp:BoundField DataField="n1_peso" HeaderText="Peso" />
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <HeaderStyle BackColor="#3431FF" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#F2F2F2" ForeColor="Black" HorizontalAlign="Right" />
                                        <RowStyle BackColor="#F2F2F2" />
                                        <SelectedRowStyle BackColor="#75F94D" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                        <SortedAscendingHeaderStyle BackColor="#848384" />
                                        <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                        <SortedDescendingHeaderStyle BackColor="#575357" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="GrdNivel2" runat="server" AutoGenerateColumns="False" DataKeyNames="n2_id,n2_texto,n2_peso" BackColor="White" BorderStyle="None" CellPadding="4" ForeColor="Black" GridLines="Vertical" Caption="Palavras de Nível II" AllowSorting="true" >
                                        <Columns>
                                            <asp:BoundField DataField="n2_id" HeaderText="ID" />
                                            <asp:BoundField DataField="n2_texto" HeaderText="Palavra" />
                                            <asp:BoundField DataField="n2_peso" HeaderText="Peso" />
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <HeaderStyle BackColor="#3431FF" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#F2F2F2" ForeColor="Black" HorizontalAlign="Right" />
                                        <RowStyle BackColor="#F2F2F2" />
                                        <SelectedRowStyle BackColor="#75F94D" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                        <SortedAscendingHeaderStyle BackColor="#848384" />
                                        <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                        <SortedDescendingHeaderStyle BackColor="#575357" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                         &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <table align="center">
                            <tr>
                                <td style="color:#3431FF" align="right">Classificação : </td>
                                <td><asp:Label ID="lblClassificacao" runat="server" Text=""></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                         &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <table>
                            <tr>
                                <td style="color:#3431FF">Classificação</td>
                                <td title="Escolha a Classificação">
                                    <asp:DropDownList ID="ddlClassificacao" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="ddlClassificacao_SelectedIndexChanged"></asp:DropDownList>
                                </td>                            
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                         &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                         &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <table>
                            <tr>
                                <td title="Pressione o Botão Para Gravar a Classificação do Edital">
                                    <asp:ImageButton ID="BtnGrava" runat="server" ImageUrl="./imagens/gravar.png" OnClick="BtnGrava_Click" Visible="False"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                         &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="color:#3431FF">Data Início</td>
                    <td>
                        <asp:TextBox ID="TxtDataIni" runat="server" Width="135px" Height="25px" TextMode="Date" required="required"></asp:TextBox>
                    </td>
                    <td style="color:#3431FF">Data Término</td>
                    <td>
                        <asp:TextBox ID="TxtDataFim" runat="server" Width="135px" Height="25px" TextMode="Date" required="required"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <table>
                            <tr>
                                <td style="color:#3431FF">Classificados</td>
                                <td>
                                    <asp:RadioButton ID="rdbSim" GroupName="Classificado" runat="server" Text="Sim" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdbNao" GroupName="Classificado" runat="server" Text="Não" Checked="True" />
                                </td>                                                
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:CheckBox ID="ChbDelete" ValidationGroup="valid" runat="server" OnCheckedChanged="ChbDelete_CheckedChanged" Text="Deletados" AutoPostBack="True" style="color:#3431FF" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:CheckBox ID="ChbEditados" runat="server" OnCheckedChanged="ChbEditados_CheckedChanged" Text="Editados" AutoPostBack="True" style="color:#3431FF"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:CheckBox ID="ChbAprovado"  runat="server" OnCheckedChanged="ChbAprovado_CheckedChanged" Text="Aprovado" AutoPostBack="True" style="color:#3431FF"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <table>
                            <tr>
                                <td style="color:#3431FF">Origem</td>
                                <td title="Escolha a Origem dos Editais">
                                    <asp:DropDownList ID="ddlOrigem" runat="server"></asp:DropDownList>
                                </td>                            
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                         &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <table>
                            <tr>
                                <td title="Pressione o Botão Para Buscar os Editais">
                                    <asp:ImageButton ID="btnBuscar" runat="server" ImageUrl="imagens/buscar.png" OnClick="btnBuscar_Click"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table align="center">
                <tr>
                    <td>
                        <asp:GridView ID="GrdEdital" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" BackColor="White" BorderStyle="None" CellPadding="4" ForeColor="Black" GridLines="Vertical" OnRowCommand="GrdEdital_RowCommand" AllowPaging="True" onpageindexchanging="GrdEdital_PageIndexChanging" PageSize="12">
                            <Columns>
                                <asp:ButtonField ButtonType="Button" CommandName="Editar" Text="Editar"/>
                                <asp:ButtonField ButtonType="Button" CommandName="Deletar" Text="Deletar"/>
                                <asp:ButtonField ButtonType="Button" CommandName="Aprovado" Text="Aprovado"/>
                                <asp:BoundField DataField="DataCadastro" HeaderText="Data Cadastro" />
                                <asp:BoundField DataField="OrgaoResponsavel" HeaderText="Orgão Responsável" />
                                <asp:BoundField DataField="Descricao" HeaderText="Origem" />
                                <asp:BoundField DataField="Codigo" HeaderText="Classificação" />
                                <asp:BoundField DataField="Objeto" HeaderText="Objeto" />
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#3431FF" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#F2F2F2" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F2F2F2" />
                            <PagerSettings Position="Bottom" Mode="NextPreviousFirstLast"
                            PreviousPageText="<img src='imagens/setasimplesesquerda.png' border='0' title='Página Anterior'/>"
                            NextPageText="<img src='imagens/setasimplesdireita.png' border='0' title='Próxima Página'/>"
                            FirstPageText="<img src='imagens/setaduplaesquerda.png' border='0' title='Primeira Página'/>"
                            LastPageText="<img src='imagens/setadupladireita.png' border='0' title='Última Página'/>" 
                            PageButtonCount="15" />
                            <SelectedRowStyle BackColor="#75F94D" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnGerar" />
                    <asp:PostBackTrigger ControlID="BtnGrava" />
                    <asp:PostBackTrigger ControlID="btnBuscar" />
                    <asp:PostBackTrigger ControlID="GrdEdital" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
</asp:Content>
