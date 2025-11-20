using System.ComponentModel.DataAnnotations;

public class RequiredIfTrueAttribute : ValidationAttribute
{
    private readonly string _nomePropriedade;

    // Construtor: Passamos o nome da propriedade que será verificada
    public RequiredIfTrueAttribute(string nomePropriedade)
    {
        _nomePropriedade = nomePropriedade;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Obter a instância da classe (o seu modelo)
        var instance = validationContext.ObjectInstance;
        var type = instance.GetType();

        // Obter o valor da propriedade de dependência (a que deve ser 'true')
        var propertyToCheck = type.GetProperty(_nomePropriedade);
        if (propertyToCheck == null)
        {
            // Tratar erro se o nome da propriedade estiver incorreto
            throw new ArgumentException($"Propriedade '{_nomePropriedade}' não encontrada.");
        }

        bool isRequired = (bool)propertyToCheck.GetValue(instance);

        if (isRequired) // Propriedade de dependência (propertyCheck) é TRUE?
        {
            // Se for TRUE, é verificado se o campo atual (value) está preenchido
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                // Se o campo for nulo/vazio, a validação falha
                return new ValidationResult(
                    ErrorMessage ?? $"{validationContext.DisplayName} é obrigatório."
                );
            }
        }

        // Se 'isRequired' for FALSE, ou se o(s) campo(s) que depende(m) estiver(em) preenchido(s), a validação é bem-sucedida.
        return ValidationResult.Success;
    }
}