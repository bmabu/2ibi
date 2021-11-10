# CSharp-Dev-Challenge
C# Dev Challenge by 2iBi


Desafio concluído
Mas, alguns países exportam com problemas porque não tem algumas propriedades como border(ilhas), cioc ou capital.

# Inside de Project:

1. Busca todos os países registados na API. Usei o datatables.net para apresentar as informações.
2. Infelizmente ainda não pude fazer o progress bar ou spinner para mostrar enquanto os dados são processados no server side.
3. Ao clicar o botão para exportar para algum tipo de ficheiro, os dados da primeira coluna são armazenados em um array de strings para fazerem a pesquisa na API, um por um. E no final juntar os dados da resposta, chamar o método específico para o tipo de ficheiro pedido.
4. ao buscar os dados, para o c# versão 4.6 e maior, o melhor tipo de dados para receber a resposta é dynamic porque o tipo de dados não precisa predefinir o tipo de dado, o compilador decide na hora da execução.
5. Métodos auxiliares para desserializar os arrays de objectos JSON foram guardados na pasta e classe fileMakers.


# Como usar o programa?
Pesquisa pelo país que quer, a cada click do mouse vai recebendo os dados que combinam com o texto
Clica no botão para exportar os dados no formato que precisar.

Repositório:               https://github.com/bmabu/CSharp-Dev-Challenge.git
hospedado em:         fam.bitsol-fleet.com

Meus perfis:
Github:            https://github.com/bmabu
linkedin:           linkedin.com/in/benildo-mabunda-45a050b9
Projectos:        www.bitsol-fleet.com (app para gestão de transportes)
website:            About.me/benildomabunda
instagram:       b_mabunda
