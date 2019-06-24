<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Terrapar.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - Terrapar</title>
    <script src="http://code.jquery.com/jquery-1.10.2.min.js"></script>

    <style>
        
        html, body
        {
            font-family: "Helvetica Neue", Helvetica, sans-serif;
            color: #444;
            -webkit-font-smoothing: antialiased;
            background: #f0f0f0;
        }
        
        body
        {
            line-height: 1;
        }
        
        form
        {
            margin: 0 auto;
            margin-top: 20px;
        }
        
        h1
        {
            width:520px;
            margin:15px auto;
            text-align:center;
        }
        
        label
        {
            color: #555;
            display: inline-block;
            margin-left: 18px;
            padding-top: 10px;
            font-size: 14px;
        }
        
        input[type=submit] {
            float: right;
            margin-right: 20px;
            margin-top: 20px;
            width: 80px;
            height: 30px;
            font-size: 14px;
            font-weight: bold;
            color: #fff;
            background-color: #acd6ef;
            background-image: -webkit-gradient(linear, left top, left bottom, from(#acd6ef), to(#6ec2e8));
            background-image: -moz-linear-gradient(top left 90deg, #acd6ef 0%, #6ec2e8 100%);
            background-image: linear-gradient(top left 90deg, #acd6ef 0%, #6ec2e8 100%);
            border-radius: 30px;
            border: 1px solid #66add6;
            box-shadow: 0 1px 2px rgba(0, 0, 0, .3), inset 0 1px 0 rgba(255, 255, 255, .5);
            cursor: pointer;
        }
        
        input[type=text], input[type=password] {
            color: #777;
            padding-left: 10px;
            margin: 10px;
            margin-top: 12px;
            margin-left: 18px;
            width: 290px;
            height: 35px;
            border: 1px solid #c7d0d2;
            border-radius: 2px;
            box-shadow: inset 0 1.5px 3px rgba(190, 190, 190, .4), 0 0 0 5px #f5f7f8;
            -webkit-transition: all .4s ease;
            -moz-transition: all .4s ease;
            transition: all .4s ease;
        }
        
            input[type=text]:focus, input[type=password]:focus {
                border: 1px solid #a8c9e4;
                box-shadow: inset 0 1.5px 3px rgba(190, 190, 190, .4), 0 0 0 5px #e6f2f9;
            }
        
        input {
            font-family: "Helvetica Neue", Helvetica, sans-serif;
            font-size: 12px;
            outline: none;
        }
        
        .box-login
        {
            width: 340px;
            height: 280px;
            margin: 20px auto;
            background: #fff;
            border-radius: 3px;
            border: 1px solid #ccc;
            box-shadow: 0 1px 2px rgba(0, 0, 0, .1);
        }
        
        #lower {
            background: #ecf2f5;
            width: 100%;
            height: 69px;
            margin-top: 20px;
            box-shadow: inset 0 1px 1px #fff;
            border-top: 1px solid #ccc;
            border-bottom-right-radius: 3px;
            border-bottom-left-radius: 3px;
        }
        
    </style>

</head>
<body>
    
    <h1>Terrapar - Sistema administrativo</h1>
    <div class="box-login">
        <form id="form1" runat="server">
            <label for="txtPlaca">Placa:</label>
            <asp:TextBox runat="server" ID="txtPlaca"></asp:TextBox>
            <label for="txtSenha">Senha:</label>
            <asp:TextBox runat="server" ID="txtSenha" TextMode="Password"></asp:TextBox><br />
            <asp:Label runat="server" ID="lblStatusLogin" Visible="false"></asp:Label>
            <div id="lower">
			    <asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="Entrar" />
		    </div>
        </form>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            var txtPlaca = $('[id$=txtPlaca]'), txtSenha = $('[id$=txtSenha]');

            if ('placeholder' in document.createElement('input')) {
                txtPlaca.attr('placeholder', 'Placa');
                txtSenha.attr('placeholder', 'Senha');
            } else {
                if ($.trim(txtPlaca.val()) == '')
                    txtPlaca.val('Placa');
                if ($.trim(txtSenha.val()) == '')
                    txtSenha.val('Senha');

                txtPlaca.on('focus', function () {
                    if ($.trim($(this).val()) == 'Placa')
                        $(this).val('');
                }).on('blur', function () {
                    if ($.trim($(this).val()) == '')
                        $(this).val('Placa');
                });
                txtSenha.on('focus', function () {
                    if ($.trim($(this).val()) == 'Senha')
                        $(this).val('');
                }).on('blur', function () {
                    if ($.trim($(this).val()) == '')
                        $(this).val('Senha');
                });
            }
        });
    </script>
</body>
</html>
