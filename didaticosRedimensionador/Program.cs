using System;
using System.Drawing;
using System.Threading;

namespace didaticos.redimensionador
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("iniciando redimensionador");
            Thread thread = new Thread(Redimensionar);
            thread.Start();
            Console.Read();
        }

        static void Redimensionar()
        {
            #region "Disretorios"
            string diretorio_entrada = "Arquivos_Entrada";
            string diretorio_finalizado = "Arquivos_Finalizado";
            string diretorio_redimensionado = "Arquivos_Redimensionados";
            string diretorio_redimensionado_instagram = "Arquivos_Redimensionados_Instagram";


            if (!Directory.Exists(diretorio_entrada))
            {
                Directory.CreateDirectory(diretorio_entrada);
            }
            if (!Directory.Exists(diretorio_redimensionado))
            {
                Directory.CreateDirectory(diretorio_redimensionado);
            }
            if (!Directory.Exists(diretorio_finalizado))
            {
                Directory.CreateDirectory(diretorio_finalizado);
            }
            if (!Directory.Exists(diretorio_redimensionado_instagram))
            {
                Directory.CreateDirectory(diretorio_redimensionado_instagram);
            }
            #endregion

            FileStream fileStream;
            FileInfo fileInfo;
            while (true) 
            {
                //Meu programa vai olhar para a pasta de entrada
                //Se tiver arquivo, ele irá redimencionar

                var arquivoEntrada = Directory.EnumerateFiles(diretorio_entrada);

                //ler o tamanho que irá redimencionar
                int novaAltura = 200;
                int alturaInstagram = 1080;

                foreach (var arquivo in arquivoEntrada) {

                    fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fileInfo = new FileInfo(arquivo);
                    Console.WriteLine(fileInfo.Name);
                    string caminho = Environment.CurrentDirectory + @"\" + diretorio_redimensionado + @"\"
                    + DateTime.Now.Millisecond.ToString() + "_" + fileInfo.Name;
                    string caminhoInstagram = Environment.CurrentDirectory + @"\" + diretorio_redimensionado_instagram + @"\"
                    + DateTime.Now.Millisecond.ToString() + "_insta" + fileInfo.Name;

                    //Redimenciona + copia os arquivos redimensionado para a pasta de redimensionados
                    Redimensionador(Image.FromStream(fileStream),novaAltura, caminho);
                    RedimensionadorInstagram(Image.FromStream(fileStream), alturaInstagram, caminhoInstagram);
                    

                    //fecha o arquivo
                    fileStream.Close();

                    //move arquivos de entrada para a pasta de finalizado
                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorio_finalizado+
                    @"\"+ fileInfo.Name;
                    
                    fileInfo.MoveTo(caminhoFinalizado);


                }
                Thread.Sleep(new TimeSpan(0,0,5)); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagem">Imagem a ser redimensina</param>
        /// <param name="altura">Altura que desejamos redimencionar</param>
        /// <param name="caminho">caminho onde iremos gravar o arquivo redimencionado</param>
        /// <returns></returns>
        static void Redimensionador(Image imagem, int altura, string caminho )
        {
            double ratio = (double)altura / (double)imagem.Height;
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImage = new Bitmap(novaLargura, novaAltura);

            using(Graphics g = Graphics.FromImage(novaImage)) 
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);            
            }
            novaImage.Save(caminho);
            imagem.Dispose();

        }

        static void RedimensionadorInstagram(Image imagem, int altura, string caminho)
        {
            double ratio = (double)altura / (double)imagem.Height;
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImage = new Bitmap(novaLargura, novaAltura);

            using (Graphics g = Graphics.FromImage(novaImage))
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }
            novaImage.Save(caminho);
            imagem.Dispose();

        }
    }


}
