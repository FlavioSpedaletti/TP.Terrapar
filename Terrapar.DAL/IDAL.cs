using System.Collections.Generic;

//**************************************************************
//LINKS
//http://www.macoratti.net/aspn_csq.htm

//http://www.macoratti.net/09/08/c_mysql1.htm
//http://www.macoratti.net/09/09/c_mysql2.htm
//http://www.macoratti.net/09/10/c_mysql3.htm

//http://www.devmedia.com.br/arquitetura-em-camadas-com-c/12037
//**************************************************************

namespace Terrapar.DAL
{
    public interface IDAL<T> where T : class
    {
        long Adicionar(T DTO);
        int Deletar(T DTO);
        int Atualizar(T DTO);
        IEnumerable<T> Listar(T parametros);
        T RecuperarPorID(int id);
    }
}