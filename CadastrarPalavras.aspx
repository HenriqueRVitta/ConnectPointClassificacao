<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CadastrarPalavras.aspx.cs" Inherits="Classificacao.CadastrarPalavras" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div>
        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1">
           <ajaxToolkit:TabPanel runat="server" HeaderText="Palavras" ID="TabPnlPalavras">
                <ContentTemplate>
                    <asp:Panel ID="PnlPalvras" runat="server">
                        <asp:UpdatePanel ID="UpdPalavras" runat="server">
                            <ContentTemplate>
                                <table align="center" >
                                    <tr>
                                        <td colspan="2" align="center">
                                            <h2 style="color:#3431FF">Palavras Filtro de Licitações</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color:#3431FF">Identificação</td>
                                        <td>
                                            <asp:Label ID="lblId" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color:#3431FF">Palavra</td>
                                        <td title="Digite a Palavra Filtro das Licitações">
                                            <asp:TextBox runat="server" MaxLength="30" Width="300px" ID="txtPalavra"></asp:TextBox>   
                                        </td>
                                        <td>
                                            <table align="center">
                                                <tr>
                                                    <td title="Filtar Palavra Filtro de Licitações Começando Com">
                                                        <asp:Button ID="btnNovo" runat="server" Text=" < " OnClick="btnNovo_Click" />
                                                    </td>
                                                    <td title="Filtar Palavra Filtro de Licitações Contem">
                                                        <asp:Button ID="btnContem" runat="server" Text=" = " OnClick="btnContem_Click" />
                                                    </td>
                                                    <td title="Filtar Palavra Filtro de Licitações Terminando Com">
                                                        <asp:Button ID="btnFim" runat="server" Text=" > " OnClick="btnFim_Click" />
                                                    </td>
                                                </tr>
                                            </table>                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <table>
                                                <tr>
                                                    <td title="Pressione o Botão Para Gravar o Item">
                                                        <asp:ImageButton ID="BtnGrava" runat="server" ImageUrl="./imagens/gravar.png" OnClick="BtnGrava_Click"/>
                                                    </td>
                                                    <td title="Pressione o Botão Para Excluir o Item">
                                                        <asp:ImageButton ID="BtnExcluir" runat="server" ImageUrl="./imagens/excluir.png" OnClick="BtnExcluir_Click"/>
                                                    </td>
                                                    <td title="Pressione o Botão Para Limpar Dados da Tela">
                                                        <asp:ImageButton ID="BtnLimpa" runat="server" ImageUrl="./imagens/limpar.png" OnClick="BtnLimpa_Click"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                </table>
                                <table  align="center">
                                    <tr>
                                        <td>
                                            <div style="width:100%; height:100%; overflow-y: scroll;">
                                                <asp:GridView ID="GrdPalavra" runat="server" AutoGenerateColumns="False" BackColor="White" DataKeyNames="pl_id" BorderStyle="Groove" GridLines="Vertical" CellPadding="4" ForeColor="Black"  Width="290px" AllowSorting="True" OnSorting="GrdPalavra_Sorting" OnSelectedIndexChanged="GrdPalavra_SelectedIndexChanged" AllowPaging="True" onpageindexchanging="GrdPalavra_PageIndexChanging" PageSize="12">
                                                    <AlternatingRowStyle BackColor="White" /> 
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" HeaderText="Selecionar" ButtonType="Button" SelectText="Escolher" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="80px" />
                                                        </asp:CommandField>
                                                        <asp:BoundField DataField="pl_palavra" HeaderText="Palavra Filtro" SortExpression="pl_palavra" />
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCC99" />
                                                    <HeaderStyle BackColor="#3431FF" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#F2F2F2" ForeColor="Black" HorizontalAlign="Right" />
                                                    <RowStyle BackColor="#F2F2F2" />
                                                    <SelectedRowStyle BackColor="#75F94D" Font-Bold="True" ForeColor="White" />
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
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </ContentTemplate>       
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel runat="server" HeaderText="Excludentes" ID="TabPnlExcludentes">
                <ContentTemplate>
                    <asp:Panel ID="PnlExcludentes" runat="server">
                        <asp:UpdatePanel ID="UpdExcludentes" runat="server">
                            <ContentTemplate>
                                <table align="center" >
                                    <tr>
                                        <td colspan="2" align="center">
                                            <h2 style="color:#3431FF">Palavras Excludentes do Filtro de Licitações</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color:#3431FF">Palavra</td>
                                        <td>
                                            <asp:Label ID="LblPalavra" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color:#3431FF">Identificação</td>
                                        <td>
                                            <asp:Label ID="lblIdEx" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color:#3431FF">Palavra</td>
                                        <td title="Digite a Palavra Excludente do Filtro das Licitações">
                                            <asp:TextBox runat="server" MaxLength="30" Width="300px" ID="txtExcludente"></asp:TextBox>   
                                        </td>
                                        <td>
                                            <table align="center">
                                                <tr>
                                                    <td title="Palavra Excludente do Filtro de Licitações Começando Com">
                                                        <asp:Button ID="btnIniEx" runat="server" Text=" < " OnClick="btnIniEx_Click"/>
                                                    </td>
                                                    <td title="Palavra Excludente do Filtro de Licitações Contem">
                                                        <asp:Button ID="btnConEx" runat="server" Text=" = " OnClick="btnConEx_Click"/>
                                                    </td>
                                                    <td title="Palavra Excludente do Filtro de Licitações Terminando Com">
                                                        <asp:Button ID="btnFimEx" runat="server" Text=" &gt; " OnClick="btnFimEx_Click"/>
                                                    </td>
                                                </tr>
                                            </table>                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <table align="center">
                                                <tr>
                                                    <td title="Pressione o Botão Para Gravar o Item">
                                                        <asp:ImageButton ID="btnGravaEx" runat="server" ImageUrl="./imagens/gravar.png" OnClick="BtnGravaEx_Click"/>
                                                    </td>
                                                    <td title="Pressione o Botão Para Excluir o Item">
                                                        <asp:ImageButton ID="btnEcluirEx" runat="server" ImageUrl="./imagens/excluir.png" OnClick="BtnExcluirEx_Click"/>
                                                    </td>
                                                    <td title="Pressione o Botão Para Limpar Dados da Tela">
                                                        <asp:ImageButton ID="btnLimpaEx" runat="server" ImageUrl="./imagens/limpar.png" OnClick="BtnLimpaEx_Click"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                </table>
                                <table  align="center">
                                    <tr>
                                        <td>
                                            <div style="width:100%; height:100%; overflow-y: scroll;">
                                                <asp:GridView ID="GrdExcludente" runat="server" AutoGenerateColumns="False" BackColor="White" DataKeyNames="pe_id" BorderStyle="Groove" GridLines="Vertical" CellPadding="4" ForeColor="Black"  Width="290px" AllowSorting="True" OnSorting="GrdExcludente_Sorting" OnSelectedIndexChanged="GrdExcludente_SelectedIndexChanged" AllowPaging="True" onpageindexchanging="GrdExcludente_PageIndexChanging" PageSize="12">
                                                    <AlternatingRowStyle BackColor="White" /> 
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" HeaderText="Selecionar" ButtonType="Button" SelectText="Escolher" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="80px" />
                                                        </asp:CommandField>
                                                        <asp:BoundField DataField="pe_excludente" HeaderText="Palavra Excludente" SortExpression="pe_excludente" />
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCC99" />
                                                    <HeaderStyle BackColor="#3431ff" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#F2F2F2" ForeColor="Black" HorizontalAlign="Right" />
                                                    <RowStyle BackColor="#F2F2F2" />
                                                    <SelectedRowStyle BackColor="#75F94D" Font-Bold="True" ForeColor="White" />
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
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>
</asp:Content>
