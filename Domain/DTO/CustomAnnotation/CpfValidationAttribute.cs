using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class CpfValidationAttribute : ValidationAttribute
{
    // Mensagem de erro padrão
    private const string DefaultErrorMessage = "O CPF informado é inválido.";

    public CpfValidationAttribute() : base(DefaultErrorMessage) { }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            // O [Required] deve lidar com null/vazio.
            return ValidationResult.Success;
        }

        var cpf = value.ToString();
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return ValidationResult.Success;
        }

        // 1. Limpa o CPF (remove pontos e traços)
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        // 2. Validação básica de formato e repetição
        if (cpf.Length != 11 || TodosDigitosIguais(cpf))
        {
            return new ValidationResult(ErrorMessage);
        }

        // 3. Calcula e verifica o primeiro Dígito Verificador (DV1)
        var tempCpf = cpf.Substring(0, 9);
        var dv1 = CalcularDigitoVerificador(tempCpf, 10);
        if (dv1.ToString() != cpf.Substring(9, 1))
        {
            return new ValidationResult(ErrorMessage);
        }

        // 4. Calcula e verifica o segundo Dígito Verificador (DV2)
        tempCpf += dv1;
        var dv2 = CalcularDigitoVerificador(tempCpf, 11);
        if (dv2.ToString() != cpf.Substring(10, 1))
        {
            return new ValidationResult(ErrorMessage);
        }

        // 5. CPF Válido
        return ValidationResult.Success;
    }

    // --- Métodos Auxiliares do Algoritmo ---

    private bool TodosDigitosIguais(string cpf)
    {
        return cpf.Distinct().Count() == 1;
    }

    private int CalcularDigitoVerificador(string tempCpf, int multiplicadorInicial)
    {
        var soma = 0;
        var multiplicador = multiplicadorInicial;

        for (var i = 0; i < tempCpf.Length; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador;
            multiplicador--;
        }

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}