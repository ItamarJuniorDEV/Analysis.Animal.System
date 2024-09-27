using Analysis.Animal.System.Models;
using Analysis.Animal.System.Services.Interfaces;
using ClosedXML.Excel;

namespace Analysis.Animal.System.Services
{
    public class UploadDataService : IUploadDataService
    {
        private readonly IOpenAIService _openAiService;

        public UploadDataService(IOpenAIService openAIService)
        {
            _openAiService = openAIService;
        }

        public string UploadFarm(IFormFile formFile)
        {
            return $"Arquivo com as fazendo de nome {formFile.FileName} importado com sucesso.";
        }

        public string UploadSupportSheet(IFormFile formFile)
        {
            // Abre o arquivo Excel
            using (var stream = formFile.OpenReadStream())
            {
                using (var workbook = new XLWorkbook(stream))
                {
                    // Acessa a primeira planilha
                    var worksheet = workbook.Worksheet(1);

                    var rowCount = 0;
                    var farmRegisters = new List<FarmRegister>();

                    // Iterar sobre as linhas
                    foreach (var row in worksheet.RowsUsed())
                    {
                        rowCount++;
                        if (rowCount < 3)
                            continue;

                        var farmRegister = new FarmRegister
                        {
                            Fazenda = row.Cell(1).Value.ToString(),
                            Status = row.Cell(2).Value.ToString(),
                            Ultimo = row.Cell(3).Value.ToString(),
                            Antena = row.Cell(4).Value.ToString(),
                            Topologia = row.Cell(5).Value.ToString(),
                            Responsavel = row.Cell(6).Value.ToString(),
                            Data = row.Cell(7).Value.ToString(),
                            Mes = row.Cell(8).Value.ToString(),
                            SolucionadoData = row.Cell(9).Value.ToString(),
                            ContatoDaFazenda = row.Cell(10).Value.ToString(),
                            ProblemaObservado = row.Cell(11).Value.ToString(),
                            FormulaDeResolucao = row.Cell(12).Value.ToString(),
                            GravidadeDoProblema = row.Cell(13).Value.ToString(),
                            Observacao = row.Cell(14).Value.ToString()
                        };

                        farmRegisters.Add(farmRegister);
                    }

                    var msg = @"
                        Gere o html desses dados de fazendas de forma gráfica com base em todos os dados da planilhada lida
                        792 linhas e retorne em html e css com tudo num arquivo html estilizado, 
                        não coloque texto no retorno, apenas código, sem markdown (```html): 
                    ";

                    var result = string.Join("; ", farmRegisters.Select(f => @$"
                        {f.Fazenda} - {f.Status} - {f.Ultimo} - {f.Antena} - {f.Topologia} - 
                        {f.Responsavel} - {f.Data} - {f.Data} - {f.Mes} - {f.SolucionadoData} -
                        {f.ContatoDaFazenda} - {f.ProblemaObservado} - {f.FormulaDeResolucao} -
                        {f.GravidadeDoProblema} - {f.Observacao}
                    "));

                    var html = _openAiService.GenerateHtml(msg + result);
                    return html;
                }
            }
        }
    }
}