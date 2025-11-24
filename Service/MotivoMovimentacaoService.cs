using ApiHortifruti.Data.Repository.Interfaces;
using ApiHortifruti.Domain;
using ApiHortifruti.Exceptions;
using ApiHortifruti.Service.Interfaces;
using AutoMapper;

namespace ApiHortifruti.Service;

public class MotivoMovimentacaoService : IMotivoMovimentacaoService
{
    private readonly IUnityOfWork _uow;
    private readonly IMapper _mapper;

    public MotivoMovimentacaoService(IUnityOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetMotivoMovimentacaoDTO>> ObterTodosOsMotivosMovimentacaoAsync()
    {
        try
        {
            var motivos = await _uow.MotivoMovimentacao.ObterTodosAsync();
            return _mapper.Map<IEnumerable<GetMotivoMovimentacaoDTO>>(motivos);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GetMotivoMovimentacaoDTO?> ObterMotivoMovimentacaoPorIdAsync(int id)
    {
        var motivo = await _uow.MotivoMovimentacao.ObterPorIdAsync(id);
        return _mapper.Map<GetMotivoMovimentacaoDTO?>(motivo);
    }

    public async Task<GetMotivoMovimentacaoDTO> CriarMotivoMovimentacaoAsync(PostMotivoMovimentacaoDTO postMotivoMovimentacaoDTO)
    {
        var motivoMovimentacao = _mapper.Map<MotivoMovimentacao>(postMotivoMovimentacaoDTO);
        motivoMovimentacao.Ativo = true;
        
        // Adiciona ao contexto
        var criado = await _uow.MotivoMovimentacao.AdicionarAsync(motivoMovimentacao);
        
        // Salva no banco de dados (Unit of Work)
        await _uow.SaveChangesAsync(); 

        return _mapper.Map<GetMotivoMovimentacaoDTO>(criado);
    }

    public async Task AtualizarMotivoMovimentacaoAsync(int id, PutMotivoMovimentacaoDTO putMotivoMovimentacaoDTO)
    {
        if (id != putMotivoMovimentacaoDTO.Id)
        {
            throw new ArgumentException("O ID informado na URL não corresponde ao ID do corpo da requisição.");
        }

        var motivoMovimentacao = _mapper.Map<MotivoMovimentacao>(putMotivoMovimentacaoDTO);

        // Atualiza no contexto
        await _uow.MotivoMovimentacao.AtualizarAsync(motivoMovimentacao);
        
        // Salva no banco de dados
        await _uow.SaveChangesAsync();
    }

    public async Task DeletarMotivoMovimentacaoAsync(int id)
    {
        var motivoMovimentacao = await _uow.MotivoMovimentacao.ObterPorIdAsync(id);
        if (motivoMovimentacao == null) throw new NotFoundException("O 'Motivo de Movimentação' informado na requisição não existe");

        // Remove do contexto
        await _uow.MotivoMovimentacao.DeletarAsync(motivoMovimentacao);
        
        // Salva no banco de dados
        await _uow.SaveChangesAsync();
    }
}
