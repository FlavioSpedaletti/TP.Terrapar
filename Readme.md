1. Cria imagem MySql (na raíz do projeto)
docker build -t mysql-terrapar-image -f Terrapar.DAL/Dockerfile .

2. Cria container
docker run -d --rm --name mysql-terrapar-container mysql-terrapar-image
--com volume (para não perder os dados), e com porta (para expor na minha máquina)
docker run -d -v //c/projetos/TP.Terrapar/Terrapar.DAL/db/data://var/lib/mysql -p 3306:3306 --rm --name mysql-terrapar-container mysql-terrapar-image

3. Verifica container
docker ps

4. Entra no MySql
docker exec -it mysql-terrapar-container /bin/bash
mysql -uroot -pserenissima

5. Cria BD e insere dados básicos
/Terrapar.DAL/cria tabelas.sql

6. Alterar connection string para
<add name="terrapar" connectionString="Server=localhost;Database=terrapartransp;Uid=root;Pwd=serenissima;" providerName="MySql.Data.MySqlClient"/>

7. RUN!