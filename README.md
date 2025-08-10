# Banco Digital da Ana - Microsserviços .NET 8

Este projeto implementa uma plataforma bancária digital baseada em **microsserviços** com **.NET 8**, seguindo os padrões **DDD** e **CQRS**, comunicação via HTTP e integração com **SQLite**.  
O sistema é composto por 3 APIs:

1. **API Conta Corrente**  
   - Cadastro e autenticação de usuários (JWT)
   - Movimentações (depósitos e saques)
   - Consulta de saldo
   - Inativação de conta

2. **API Transferência**  
   - Transferências entre contas da mesma instituição
   - Chama a API Conta Corrente para efetuar débitos e créditos
   - Idempotência nas requisições

---

## 📦 Estrutura do Projeto

```bash
/BancoAna_ContaCorrente -> API Conta Corrente
/BancoAna_Transferencia -> API Transferência
/Application -> Camada de aplicação
/Domain -> Camada de domínio
/Infrastructure -> Camada de infraestrutura
/Data/Scripts -> Scripts SQL de criação das tabelas
/docker-compose.yaml -> Orquestração dos serviços

```
---

## 🚀 Pré-requisitos
- [Docker](https://docs.docker.com/get-docker/)  
- [Docker Compose](https://docs.docker.com/compose/install/)  
- **Opcional para desenvolvimento local:** [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

---

## ⚙️ Como subir o ambiente

1. **Clonar o repositório**
```bash
git clone https://github.com/seuusuario/banco-digital-ana.git
cd banco-digital-ana

```

🌐 Endpoints principais
API Conta Corrente
POST /api/contacorrente - Cadastrar conta

POST /api/login - Efetuar login (retorna JWT)

PATCH /api/contacorrente/inativar - Inativar conta

POST /api/movimentacao - Movimentar conta (débito/crédito)

GET /api/saldo - Consultar saldo

API Transferência
POST /api/transferencia - Efetuar transferência


Publica tópico Kafka tarifas-registradas
docker-compose.yaml

```bash

version: '3.9'

services:
  # =============================
  # Banco de dados SQLite
  # =============================
  sqlite-db:
    image: nouchka/sqlite3:latest
    container_name: sqlite-db
    volumes:
      - ./Data:/data
    stdin_open: true
    tty: true

  # =============================
  # Kafka + Zookeeper
  # =============================
  zookeeper:
    image: confluentinc/cp-zookeeper:7.4.0
    container_name: zookeeper
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181
      ZOOKEEPER_TICK_TIME: 2000

  kafka:
    image: confluentinc/cp-kafka:7.4.0
    container_name: kafka
    depends_on:
      - zookeeper
    ports:
      - "9092:9092"
    environment:
      KAFKA_BROKER_ID: 1
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://kafka:9092,PLAINTEXT_HOST://localhost:9092
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT
      KAFKA_INTER_BROKER_LISTENER_NAME: PLAINTEXT
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1

  # =============================
  # API Conta Corrente
  # =============================
  api-contacorrente:
    build:
      context: .
      dockerfile: BancoAna_ContaCorrente/Dockerfile
    container_name: api-contacorrente
    ports:
      - "5001:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Data Source=/data/contacorrente.db
    volumes:
      - ./Data:/data
    depends_on:
      - sqlite-db

  # =============================
  # API Transferência
  # =============================
  api-transferencia:
    build:
      context: .
      dockerfile: BancoAna_Transferencia/Dockerfile
    container_name: api-transferencia
    ports:
      - "5002:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Data Source=/data/transferencia.db
    volumes:
      - ./Data:/data
    depends_on:
      - sqlite-db
      - api-contacorrente
      - kafka

networks:
  default:
    driver: bridge


```
