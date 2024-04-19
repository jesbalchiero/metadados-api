using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace metadadosApi.Controllers
{
    [ApiController]
    [Route("api/metadados")]
    public class MetadadosController : ControllerBase
    {
        //GET: api/metadados
        [HttpGet]
        public async Task<Guid?> GetIdNewUser()
        {
            string cpf = "48638064004";
            string accessToken = await ObterTokenAsync();

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Fazendo a chamada GET para /cadastro/preAdmissao
            var response = await httpClient.GetAsync($"https://apimetadadosback-dev.metadados.com.br/cadastro/preAdmissao?cpf={cpf}");

            if (response.IsSuccessStatusCode)
            {
                var responseBodyList = await response.Content.ReadFromJsonAsync<List<ResponseBody>>();
                var responseBody = responseBodyList.FirstOrDefault(x => x.Pessoa.CPF == cpf);

                if (responseBody == null) throw new Exception($"Erro ao fazer a chamada GET: Não foi encontrado cadastro!");

                responseBody.Pessoa.Id = null;
                responseBody.Pessoa.CPF = "04235702009";
                responseBody.Pessoa.Nome = "Jean";
                responseBody.Pessoa.NomeCompleto = "Jean Carlo Sbalchiero";

                var modifiedBody = JsonSerializer.Serialize(responseBodyList);

                // Fazendo a chamada POST para /cadastro/preAdmissao com o corpo modificado
                var postResponse = await httpClient.PostAsync("https://apimetadadosback-dev.metadados.com.br/cadastro/preAdmissao", new StringContent(modifiedBody, Encoding.UTF8, "application/json"));

                if (postResponse.IsSuccessStatusCode)
                {
                    var postResponseBody = await postResponse.Content.ReadFromJsonAsync<ResponseBody>();

                    var idGerado = postResponseBody?.Pessoa.Id;

                    return idGerado;
                }
                else
                {
                    throw new Exception($"Erro ao fazer a chamada POST: {postResponse.StatusCode}");
                }
            }
            else
            {
                throw new Exception($"Erro ao fazer a chamada GET: {response.StatusCode}");
            }
        }

        private async Task<string> ObterTokenAsync()
        {
            using var httpClient = new HttpClient();
            var resposta = await httpClient.PostAsync("https://apimetadadosback-dev.metadados.com.br/login", new StringContent("{\"email\": \"avaliacao.api@metadados.com.br\", \"senha\": \"Aval@123\"}", Encoding.UTF8, "application/json"));
            resposta.EnsureSuccessStatusCode();
            var responseBody = await resposta.Content.ReadAsStringAsync();
            var responseToken = JsonSerializer.Deserialize<TokenResponse>(responseBody);

            return responseToken.AccessToken;
        }
    }

    public class TokenResponse
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expiracaoDoToken")]
        public DateTimeOffset ExpiracaoDoToken { get; set; }

        [JsonPropertyName("idUsuario")]
        public string IdUsuario { get; set; }

        [JsonPropertyName("nomeUsuario")]
        public string NomeUsuario { get; set; }

        [JsonPropertyName("tipoUsuario")]
        public int TipoUsuario { get; set; }

        [JsonPropertyName("permissoes")]
        public List<string> Permissoes { get; set; }
    }

    public class ResponseBody
    {
        [JsonPropertyName("pessoa")]
        public Pessoa Pessoa { get; set; }

        [JsonPropertyName("contrato")]
        public Contrato Contrato { get; set; }
        
        [JsonPropertyName("familiar")]
        public List<Familiar> Familiar { get; set; }
    }

    public class Pessoa
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("pessoa")]
        public object PessoaObjeto { get; set; }

        [JsonPropertyName("empresa")]
        public string Empresa { get; set; }

        [JsonPropertyName("cpf")]
        public string CPF { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("nomeCompleto")]
        public string NomeCompleto { get; set; }

        [JsonPropertyName("nomeSocial")]
        public object NomeSocial { get; set; }

        [JsonPropertyName("apelido")]
        public string Apelido { get; set; }

        [JsonPropertyName("dataCadastramento")]
        public DateTime DataCadastramento { get; set; }

        [JsonPropertyName("nascimento")]
        public DateTime Nascimento { get; set; }

        [JsonPropertyName("localNascimento")]
        public object LocalNascimento { get; set; }

        [JsonPropertyName("ufNascimento")]
        public string UfNascimento { get; set; }

        [JsonPropertyName("pai")]
        public string Pai { get; set; }

        [JsonPropertyName("mae")]
        public string Mae { get; set; }

        [JsonPropertyName("nomeMaeCompleto")]
        public string NomeMaeCompleto { get; set; }

        [JsonPropertyName("sexo")]
        public object Sexo { get; set; }

        [JsonPropertyName("racaCor")]
        public object RacaCor { get; set; }

        [JsonPropertyName("estadoCivil")]
        public object EstadoCivil { get; set; }

        [JsonPropertyName("grauInstrucao")]
        public object GrauInstrucao { get; set; }

        [JsonPropertyName("rua")]
        public string Rua { get; set; }

        [JsonPropertyName("nroRua")]
        public string NroRua { get; set; }

        [JsonPropertyName("complemento")]
        public object Complemento { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("cidade")]
        public string Cidade { get; set; }

        [JsonPropertyName("uf")]
        public string Uf { get; set; }

        [JsonPropertyName("cep")]
        public object Cep { get; set; }

        [JsonPropertyName("ddd")]
        public object DDD { get; set; }

        [JsonPropertyName("ramal")]
        public object Ramal { get; set; }

        [JsonPropertyName("telefone")]
        public object Telefone { get; set; }

        [JsonPropertyName("telefoneRecados")]
        public string TelefoneRecados { get; set; }

        [JsonPropertyName("dddCelular")]
        public object DDDCelular { get; set; }

        [JsonPropertyName("telefoneCelular")]
        public object TelefoneCelular { get; set; }

        [JsonPropertyName("email")]
        public object Email { get; set; }

        [JsonPropertyName("emailCorporativo")]
        public object EmailCorporativo { get; set; }

        [JsonPropertyName("identidade")]
        public string Identidade { get; set; }

        [JsonPropertyName("tipoIdentidade")]
        public string TipoIdentidade { get; set; }

        [JsonPropertyName("orgaoEmissor")]
        public object OrgaoEmissor { get; set; }

        [JsonPropertyName("ufIdentidade")]
        public string UfIdentidade { get; set; }

        [JsonPropertyName("dataIdentidade")]
        public DateTime DataIdentidade { get; set; }

        [JsonPropertyName("regIdCivilNr")]
        public object RegIdCivilNr { get; set; }

        [JsonPropertyName("regIdCivilOe")]
        public object RegIdCivilOe { get; set; }

        [JsonPropertyName("regIdCivilDe")]
        public object RegIdCivilDe { get; set; }

        [JsonPropertyName("deficienteFisico")]
        public object DeficienteFisico { get; set; }

        [JsonPropertyName("defAuditivaeSocial")]
        public string DefAuditivaeSocial { get; set; }

        [JsonPropertyName("defIntelectualeSocial")]
        public string DefIntelectualeSocial { get; set; }

        [JsonPropertyName("defMentaleSocial")]
        public string DefMentaleSocial { get; set; }

        [JsonPropertyName("defMotoraeSocial")]
        public string DefMotoraeSocial { get; set; }

        [JsonPropertyName("defVisualeSocial")]
        public string DefVisualeSocial { get; set; }

        [JsonPropertyName("benefReabilitado")]
        public string BenefReabilitado { get; set; }

        [JsonPropertyName("cotapessoaDefReab")]
        public string CotapessoaDefReab { get; set; }

        [JsonPropertyName("conselhoClasse")]
        public object ConselhoClasse { get; set; }

        [JsonPropertyName("registroConselho")]
        public object RegistroConselho { get; set; }

        [JsonPropertyName("dataRegistro")]
        public object DataRegistro { get; set; }

        [JsonPropertyName("cartTrabDigital")]
        public string CartTrabDigital { get; set; }

        [JsonPropertyName("nroCartTrab")]
        public object NroCartTrab { get; set; }

        [JsonPropertyName("serieCartTrab")]
        public object SerieCartTrab { get; set; }

        [JsonPropertyName("ufCartTrab")]
        public object UfCartTrab { get; set; }

        [JsonPropertyName("dataCartTrab")]
        public object DataCartTrab { get; set; }

        [JsonPropertyName("pis")]
        public object PIS { get; set; }

        [JsonPropertyName("dataPis")]
        public object DataPIS { get; set; }

        [JsonPropertyName("tituloEleitoral")]
        public object TituloEleitoral { get; set; }

        [JsonPropertyName("zonaEleitoral")]
        public object ZonaEleitoral { get; set; }

        [JsonPropertyName("secaoEleitoral")]
        public object SecaoEleitoral { get; set; }

        [JsonPropertyName("sindicato")]
        public object Sindicato { get; set; }

        [JsonPropertyName("cnae")]
        public object CNAE { get; set; }

        [JsonPropertyName("habilitacaoProfissional")]
        public object HabilitacaoProfissional { get; set; }

        [JsonPropertyName("categoriaHabilitacao")]
        public object CategoriaHabilitacao { get; set; }

        [JsonPropertyName("registroHabilitacao")]
        public object RegistroHabilitacao { get; set; }

        [JsonPropertyName("ufHabilitacao")]
        public object UfHabilitacao { get; set; }

        [JsonPropertyName("expedicaoHabilitacao")]
        public object ExpedicaoHabilitacao { get; set; }

        [JsonPropertyName("validadeHabilitacao")]
        public object ValidadeHabilitacao { get; set; }

        [JsonPropertyName("nacionalidade")]
        public string Nacionalidade { get; set; }

        [JsonPropertyName("paisNascimento")]
        public int PaisNascimento { get; set; }

        [JsonPropertyName("paisNacionalidade")]
        public int PaisNacionalidade { get; set; }

        [JsonPropertyName("paisResidencia")]
        public int PaisResidencia { get; set; }

        [JsonPropertyName("tipoLogradouro")]
        public string TipoLogradouro { get; set; }

        [JsonPropertyName("anoChegadaBrasil")]
        public object AnoChegadaBrasil { get; set; }

        [JsonPropertyName("dataChegadaBrasil")]
        public object DataChegadaBrasil { get; set; }

        [JsonPropertyName("clasTrabEstrangeiro")]
        public object ClasTrabEstrangeiro { get; set; }

        [JsonPropertyName("regEstrangeiroNr")]
        public object RegEstrangeiroNr { get; set; }

        [JsonPropertyName("regEstrangeiroOe")]
        public object RegEstrangeiroOe { get; set; }

        [JsonPropertyName("regEstrangeiroDe")]
        public object RegEstrangeiroDe { get; set; }

        [JsonPropertyName("unidadeOperador")]
        public object UnidadeOperador { get; set; }

        [JsonPropertyName("contratoOperador")]
        public object ContratoOperador { get; set; }
    }

    public class Contrato
    {
        [JsonPropertyName("unidade")]
        public string Unidade { get; set; }

        [JsonPropertyName("contrato")]
        public object ContratoObjeto { get; set; }

        [JsonPropertyName("observacoesContrato")]
        public object ObservacoesContrato { get; set; }

        [JsonPropertyName("estabelecimento")]
        public string Estabelecimento { get; set; }

        [JsonPropertyName("situacaoPreAdmissao")]
        public string SituacaoPreAdmissao { get; set; }

        [JsonPropertyName("dataAdmissao")]
        public DateTime DataAdmissao { get; set; }

        [JsonPropertyName("tipoEmprego")]
        public object TipoEmprego { get; set; }

        [JsonPropertyName("percInsalubridade")]
        public object PercInsalubridade { get; set; }

        [JsonPropertyName("percPericulosidade")]
        public object PercPericulosidade { get; set; }

        [JsonPropertyName("nivelExposicao")]
        public string NivelExposicao { get; set; }

        [JsonPropertyName("banco")]
        public object Banco { get; set; }

        [JsonPropertyName("bancoCredor")]
        public object BancoCredor { get; set; }

        [JsonPropertyName("contaCorrente")]
        public object ContaCorrente { get; set; }

        [JsonPropertyName("opcaoPrevidencia")]
        public string OpcaoPrevidencia { get; set; }

        [JsonPropertyName("vinculoPrevidencia")]
        public string VinculoPrevidencia { get; set; }

        [JsonPropertyName("opcaoFgts")]
        public string OpcaoFgts { get; set; }

        [JsonPropertyName("dataOpcaoFgts")]
        public object DataOpcaoFgts { get; set; }

        [JsonPropertyName("cadastroEmpregadoFgts")]
        public object CadastroEmpregadoFgts { get; set; }

        [JsonPropertyName("sindicato")]
        public object Sindicato { get; set; }

        [JsonPropertyName("opcaoSindical")]
        public object OpcaoSindical { get; set; }

        [JsonPropertyName("socioSindicato")]
        public object SocioSindicato { get; set; }

        [JsonPropertyName("registro")]
        public object Registro { get; set; }

        [JsonPropertyName("tipoContrato")]
        public string TipoContrato { get; set; }

        [JsonPropertyName("inicioContrato")]
        public object InicioContrato { get; set; }

        [JsonPropertyName("terminoContrato")]
        public object TerminoContrato { get; set; }

        [JsonPropertyName("nroDiasContrato")]
        public int NroDiasContrato { get; set; }

        [JsonPropertyName("inicioProrrogacao")]
        public object InicioProrrogacao { get; set; }

        [JsonPropertyName("terminoProrrogacao")]
        public object TerminoProrrogacao { get; set; }

        [JsonPropertyName("nrodiasProrrogacao")]
        public object NrodiasProrrogacao { get; set; }

        [JsonPropertyName("salarioContratual")]
        public decimal SalarioContratual { get; set; }

        [JsonPropertyName("horasContratuais")]
        public decimal HorasContratuais { get; set; }

        [JsonPropertyName("tipoComplementoSalarial")]
        public object TipoComplementoSalarial { get; set; }

        [JsonPropertyName("tipoSalario")]
        public string TipoSalario { get; set; }

        [JsonPropertyName("cargo")]
        public object Cargo { get; set; }

        [JsonPropertyName("planoCargo")]
        public object PlanoCargo { get; set; }

        [JsonPropertyName("faixaSalarial")]
        public object FaixaSalarial { get; set; }

        [JsonPropertyName("classeSalarial")]
        public object ClasseSalarial { get; set; }

        [JsonPropertyName("funcao")]
        public object Funcao { get; set; }

        [JsonPropertyName("setor")]
        public object Setor { get; set; }

        [JsonPropertyName("escala")]
        public object Escala { get; set; }

        [JsonPropertyName("centroCusto1")]
        public object CentroCusto1 { get; set; }

        [JsonPropertyName("centroCusto2")]
        public object CentroCusto2 { get; set; }

        [JsonPropertyName("centroCusto3")]
        public object CentroCusto3 { get; set; }

        [JsonPropertyName("centroCusto4")]
        public object CentroCusto4 { get; set; }

        [JsonPropertyName("postoTrabalho")]
        public object PostoTrabalho { get; set; }

        [JsonPropertyName("classificacaoGps")]
        public object ClassificacaoGps { get; set; }

        [JsonPropertyName("classificacaoSefip")]
        public object ClassificacaoSefip { get; set; }

        [JsonPropertyName("classificacaoGerencial")]
        public object ClassificacaoGerencial { get; set; }

        [JsonPropertyName("classificacaoContabil")]
        public object ClassificacaoContabil { get; set; }

        [JsonPropertyName("vinculoEmpregaticio")]
        public object VinculoEmpregaticio { get; set; }

        [JsonPropertyName("tipoAdmissao")]
        public string TipoAdmissao { get; set; }

        [JsonPropertyName("indicativoAdmissao")]
        public string IndicativoAdmissao { get; set; }

        [JsonPropertyName("indicPrimeiroEmprego")]
        public string IndicPrimeiroEmprego { get; set; }

        [JsonPropertyName("tipoRegimeTrab")]
        public string TipoRegimeTrab { get; set; }

        [JsonPropertyName("tipoRegimePrev")]
        public string TipoRegimePrev { get; set; }

        [JsonPropertyName("naturezaAtividade")]
        public string NaturezaAtividade { get; set; }

        [JsonPropertyName("categoriaTrabalhador")]
        public string CategoriaTrabalhador { get; set; }

        [JsonPropertyName("tipoRegimeJornada")]
        public string TipoRegimeJornada { get; set; }

        [JsonPropertyName("maoDeObra")]
        public string MaoDeObra { get; set; }

        [JsonPropertyName("nrProcMenoreseSocial")]
        public object NrProcMenoreseSocial { get; set; }

        [JsonPropertyName("naturezaEstagio")]
        public object NaturezaEstagio { get; set; }

        [JsonPropertyName("nivelEstagio")]
        public object NivelEstagio { get; set; }

        [JsonPropertyName("areaAtuacaoEstagiario")]
        public object AreaAtuacaoEstagiario { get; set; }

        [JsonPropertyName("nrApoliceSeguroEstag")]
        public object NrApoliceSeguroEstag { get; set; }

        [JsonPropertyName("valorBolsaEstagiario")]
        public object ValorBolsaEstagiario { get; set; }

        [JsonPropertyName("dataTerminoEstagio")]
        public object DataTerminoEstagio { get; set; }

        [JsonPropertyName("supervisorEstagio")]
        public object SupervisorEstagio { get; set; }

        [JsonPropertyName("cpfSupervisorEstagio")]
        public object CpfSupervisorEstagio { get; set; }

        [JsonPropertyName("inscricaoInstEnsino")]
        public object InscricaoInstEnsino { get; set; }

        [JsonPropertyName("razaoSocialInstEnsino")]
        public object RazaoSocialInstEnsino { get; set; }

        [JsonPropertyName("descLogradInstEnsino")]
        public object DescLogradInstEnsino { get; set; }

        [JsonPropertyName("nroLogradInstEnsino")]
        public object NroLogradInstEnsino { get; set; }

        [JsonPropertyName("bairroInstEnsino")]
        public object BairroInstEnsino { get; set; }

        [JsonPropertyName("cepInstEnsino")]
        public object CepInstEnsino { get; set; }

        [JsonPropertyName("cidadeInstEnsino")]
        public object CidadeInstEnsino { get; set; }

        [JsonPropertyName("codMunicipInstEnsino")]
        public object CodMunicipInstEnsino { get; set; }

        [JsonPropertyName("ufInstEnsino")]
        public object UfInstEnsino { get; set; }

        [JsonPropertyName("inscricaoAgIntegr")]
        public object InscricaoAgIntegr { get; set; }

        [JsonPropertyName("razaoSocialAgIntegr")]
        public object RazaoSocialAgIntegr { get; set; }

        [JsonPropertyName("descLogradAgIntegr")]
        public object DescLogradAgIntegr { get; set; }

        [JsonPropertyName("nroLogradAgIntegr")]
        public object NroLogradAgIntegr { get; set; }

        [JsonPropertyName("bairroAgIntegr")]
        public object BairroAgIntegr { get; set; }

        [JsonPropertyName("cepAgIntegr")]
        public object CepAgIntegr { get; set; }

        [JsonPropertyName("cidadeAgIntegr")]
        public object CidadeAgIntegr { get; set; }

        [JsonPropertyName("codMunicipAgIntegr")]
        public object CodMunicipAgIntegr { get; set; }

        [JsonPropertyName("ufAgIntegr")]
        public object UfAgIntegr { get; set; }

        [JsonPropertyName("categTrabEmprOrigem")]
        public object CategTrabEmprOrigem { get; set; }

        [JsonPropertyName("inscricaoEmprOrigem")]
        public object InscricaoEmprOrigem { get; set; }

        [JsonPropertyName("dataAdmEmprOrigem")]
        public object DataAdmEmprOrigem { get; set; }

        [JsonPropertyName("matriculaEmprOrigem")]
        public object MatriculaEmprOrigem { get; set; }

        [JsonPropertyName("inscricaoEmprAnter")]
        public object InscricaoEmprAnter { get; set; }

        [JsonPropertyName("matriculaEmprAnter")]
        public object MatriculaEmprAnter { get; set; }

        [JsonPropertyName("dataAdmEmprAnter")]
        public object DataAdmEmprAnter { get; set; }

        [JsonPropertyName("inscricaoEmprCed")]
        public object InscricaoEmprCed { get; set; }

        [JsonPropertyName("matriculaEmprCed")]
        public object MatriculaEmprCed { get; set; }

        [JsonPropertyName("dataAdmEmprCed")]
        public object DataAdmEmprCed { get; set; }

        [JsonPropertyName("opcaoOnusTrabCed")]
        public object OpcaoOnusTrabCed { get; set; }

        [JsonPropertyName("motivoContratTrabTemp")]
        public object MotivoContratTrabTemp { get; set; }

        [JsonPropertyName("categTrabEmprCed")]
        public object CategTrabEmprCed { get; set; }

        [JsonPropertyName("estatutarioIndProvim")]
        public object EstatutarioIndProvim { get; set; }

        [JsonPropertyName("estatutarioTipoProvim")]
        public object EstatutarioTipoProvim { get; set; }

        [JsonPropertyName("estatutarioDataNomeacao")]
        public object EstatutarioDataNomeacao { get; set; }

        [JsonPropertyName("estatutarioDataPosse")]
        public object EstatutarioDataPosse { get; set; }

        [JsonPropertyName("estatutarioDataExercicio")]
        public object EstatutarioDataExercicio { get; set; }

        [JsonPropertyName("matriculaServidorPublico")]
        public object MatriculaServidorPublico { get; set; }

        [JsonPropertyName("fatoJustContrTrabTemp")]
        public object FatoJustContrTrabTemp { get; set; }

        [JsonPropertyName("tipoInclContrTrabTemp")]
        public object TipoInclContrTrabTemp { get; set; }

        [JsonPropertyName("cpfTrabSubstituido")]
        public object CpfTrabSubstituido { get; set; }

        [JsonPropertyName("estatutarioNrProcJud")]
        public object EstatutarioNrProcJud { get; set; }

        [JsonPropertyName("estatutarioTipoPlano")]
        public object EstatutarioTipoPlano { get; set; }

        [JsonPropertyName("carreiraPublica")]
        public object CarreiraPublica { get; set; }

        [JsonPropertyName("ingressoCarreiraPublica")]
        public object IngressoCarreiraPublica { get; set; }

        [JsonPropertyName("tipoContratoTempoParcial")]
        public object TipoContratoTempoParcial { get; set; }

        [JsonPropertyName("emprestimoConsigGarantiaFgts")]
        public object EmprestimoConsigGarantiaFgts { get; set; }

        [JsonPropertyName("matriculaInstitConsigCadCaixa")]
        public object MatriculaInstitConsigCadCaixa { get; set; }

        [JsonPropertyName("nroContratEmprestimoConsig")]
        public object NroContratEmprestimoConsig { get; set; }

        [JsonPropertyName("tipoInscricaoEmprAnter")]
        public object TipoInscricaoEmprAnter { get; set; }
    }

    public class Familiar
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("nomeCompleto")]
        public string NomeCompleto { get; set; }

        [JsonPropertyName("cpf")]
        public string CPF { get; set; }

        [JsonPropertyName("nascimento")]
        public object Nascimento { get; set; }

        [JsonPropertyName("sexo")]
        public object Sexo { get; set; }

        [JsonPropertyName("estadoCivil")]
        public object EstadoCivil { get; set; }

        [JsonPropertyName("grauInstrucao")]
        public object GrauInstrucao { get; set; }

        [JsonPropertyName("nacionalidade")]
        public object Nacionalidade { get; set; }

        [JsonPropertyName("emDiaComVacinas")]
        public object EmDiaComVacinas { get; set; }

        [JsonPropertyName("frequentaEscola")]
        public object FrequentaEscola { get; set; }

        [JsonPropertyName("grauDependencia")]
        public object GrauDependencia { get; set; }

        [JsonPropertyName("grauParentesco")]
        public object GrauParentesco { get; set; }

        [JsonPropertyName("depPlanoPrivadoSaude")]
        public object DepPlanoPrivadoSaude { get; set; }

        [JsonPropertyName("depIncapazParaTrab")]
        public object DepIncapazParaTrab { get; set; }

        [JsonPropertyName("tipoDependenteeSocial")]
        public object TipoDependenteeSocial { get; set; }

        [JsonPropertyName("salFamilia")]
        public object SalFamilia { get; set; }

        [JsonPropertyName("auxilioCreche")]
        public object AuxilioCreche { get; set; }

        [JsonPropertyName("dependenteIrf")]
        public object DependenteIRF { get; set; }

        [JsonPropertyName("benefPrevidencia")]
        public object BenefPrevidencia { get; set; }

        [JsonPropertyName("benefSeguro")]
        public object BenefSeguro { get; set; }

        [JsonPropertyName("percBenefSeguro")]
        public object PercBenefSeguro { get; set; }
    }
}