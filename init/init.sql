CREATE TABLE contrato (
    id UUID PRIMARY KEY,
    cliente_cpf_cnpj VARCHAR(20) NOT NULL,
    valor_total NUMERIC(15, 2) NOT NULL,
    taxa_mensal NUMERIC(5, 2) NOT NULL,
    prazo_meses INTEGER NOT NULL,
    data_vencimento_primeira_parcela TIMESTAMP WITH TIME ZONE NOT NULL,

    tipo_veiculo VARCHAR(20) NOT NULL,
    CONSTRAINT chk_tipo_veiculo CHECK (
        tipo_veiculo IN ('AUTOMOVEL', 'MOTO', 'CAMINHAO')
    ),

    condicao_veiculo VARCHAR(20) NOT NULL,
    CONSTRAINT chk_condicao_veiculo CHECK (
        condicao_veiculo IN ('NOVO', 'USADO', 'SEMINOVO')
    )
);

CREATE TABLE pagamento (
    id UUID PRIMARY KEY,
    data_pagamento DATE NOT NULL,
    valor_pago NUMERIC(15, 2) NOT NULL,
    status_pagamento VARCHAR(20) NOT NULL,
    CONSTRAINT chk_status_pagamento CHECK (
        status_pagamento IN ('EM_DIA', 'ATRASADO', 'ANTECIPADO')
    ),

    contrato_id UUID NOT NULL,
    CONSTRAINT fk_contrato
        FOREIGN KEY (contrato_id)
        REFERENCES contrato(id)
        ON DELETE CASCADE
);