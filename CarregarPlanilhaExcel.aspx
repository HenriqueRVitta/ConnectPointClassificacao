<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CarregarPlanilhaExcel.aspx.cs" Inherits="Classificacao.CarregarPlanilhaExcel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div>
            <table align="center">
                <tr>
                    <td colspan="2" align="center">
                        <h2 style="color:#3431FF">Carregar Planilha Excel</h2>
                    </td>
                </tr>
                <tr>
                    <td> Favor Escolher a Planilha a ser Carregada</td>
                    <td>

                       <asp:FileUpload ID="FileUpload1" runat="server" />
                    <td>
                    <td>
                        <asp:ImageButton ID="BtnCarregar" runat="server" ImageUrl="./imagens/upload.png" OnClick="BtnCarregar_Click"/>
                    </td>
               </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:ImageButton ID="BtnGravar" runat="server" ImageUrl="./imagens/gravar.png" OnClick="BtnGravar_Click"/>
                    </td>
                </tr>
            </table>
        </div>
        <div class="row-cols-1">
            <asp:label style="font-size: 20px; color: blue;" runat="server" ID="TotalContador" Text="Total de Registros Gravados: 0"></asp:label>
        </div>
</asp:Content>
