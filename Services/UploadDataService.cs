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

        public void GenerateAssistantData(IFormFile formFile)
        {
            var farmRegisters = GetFarmRegisters(formFile);

            using (StreamWriter writer = new StreamWriter("data_assistant.txt"))
            {
                foreach (var f in farmRegisters)
                {
                    var text = @$"Fazenda: {f.Fazenda} - Status: {f.Status} - Ultimo: {f.Ultimo} - Antena: {f.Antena} - Topologia: {f.Topologia} - Responsavel: {f.Responsavel} - Data: {f.Data} - Mês: {f.Mes} - Data solucionado: {f.SolucionadoData} - Contato da fazenda: {f.ContatoDaFazenda} - Problema observado: {f.ProblemaObservado} - Formula de resolução: {f.FormulaDeResolucao} - Gravidade do problema: {f.GravidadeDoProblema} - Observação: {f.Observacao}";

                    writer.WriteLine(text);
                }

                GenerateTotals(farmRegisters, writer);
            }
        }

        public void GenerateTotals(IList<FarmRegister> farmRegisters, StreamWriter writer)
        {
            // Total por antena
            var antennaProblems = farmRegisters
                .GroupBy(x => x.Antena)
                .Select(x => new
                {
                    x.FirstOrDefault()?.Antena,
                    Count = x.Count()
                })
                .ToList();

            foreach (var antennaProblem in antennaProblems)
            {
                var text = $"A antena {antennaProblem.Antena} teve o total de {antennaProblem.Count} atendimentos";
                writer.WriteLine(text);
            }

            // Total por fazenda
            var farmProblems = farmRegisters
                .GroupBy(x => x.Fazenda)
                .Select(x => new
                {
                    x.FirstOrDefault()?.Fazenda,
                    Count = x.Count()
                })
                .ToList();

            foreach (var farmProblem in farmProblems)
            {
                var text = $"A fazenda {farmProblem.Fazenda} teve o total de {farmProblem.Count} atendimentos";
                writer.WriteLine(text);
            }

            // Total de atendimentos por responsável
            var responsibleRegisters = farmRegisters
                .GroupBy(x => x.Responsavel)
                .Select(x => new
                {
                    x.FirstOrDefault()?.Responsavel,
                    Count = x.Count()
                })
                .ToList();

            foreach (var responsibleRegister in responsibleRegisters)
            {
                var text = $"O responsável {responsibleRegister.Responsavel} teve o total de {responsibleRegister.Count} atendimentos";
                writer.WriteLine(text);
            }

            // Totais das Gravidades dos problemas
            var gravityProblems = farmRegisters
                .GroupBy(x => x.GravidadeDoProblema)
                .Select(x => new
                {
                    x.FirstOrDefault()?.GravidadeDoProblema,
                    Count = x.Count()
                })
                .ToList();

            foreach (var gravityProblem in gravityProblems)
            {
                var text = $"Teve o total de {gravityProblem.Count} problemas de gravidade {gravityProblem.GravidadeDoProblema}";
                writer.WriteLine(text);
            }

            // Total de atendimentos por dia
            var servicesPerDays = farmRegisters
                .GroupBy(x => x.Data)
                .Select(x => new
                {
                    x.FirstOrDefault()?.Data,
                    Count = x.Count()
                })
                .ToList();

            foreach (var servicesPerDay in servicesPerDays)
            {
                var text = $"Teve o total de {servicesPerDay.Count} atendimentos no dia {servicesPerDay.Data}";
                writer.WriteLine(text);
            }

            // Total de atendimentos por mes
            var servicesPerMonths = farmRegisters
                .GroupBy(x => x.Mes)
                .Select(x => new
                {
                    Month = x.FirstOrDefault()?.Mes,
                    Count = x.Count()
                })
                .ToList();

            foreach (var servicesPerMonth in servicesPerMonths)
            {
                var text = $"Teve o total de {servicesPerMonth.Count} atendimentos no mês {servicesPerMonth.Month}";
                writer.WriteLine(text);
            }

            // Total por status
            var statusTotals = farmRegisters
                .GroupBy(x => x.Status)
                .Select(x => new
                {
                    x.FirstOrDefault()?.Status,
                    Count = x.Count()
                })
                .ToList();

            foreach (var statusTotal in statusTotals)
            {
                var text = $"Teve um total de {statusTotal.Count} atendimentos com o status {statusTotal.Status}";
                writer.WriteLine(text);
            }
        }

        public IList<FarmRegister> GetFarmRegisters(IFormFile formFile)
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

                    return farmRegisters;
                }
            }
        }
    }
}