Group Members


| No | Name               | ID          |
| -- | ------------------ | ----------- |
| 1  | Derartu Nigatu     | UGR/5597/15 |
| 2  | Eden Zewdu         | UGR/9956/15 |
| 3  | Estifanos Taddesse | UGR/4002/13 |
| 4  | Hermela Andargie   | UGR/9777/15 |
| 5  | Lensa Yadesa       | UGR/8593/15 |

     Enterprise Application Project Proposal

🌐 Academic Research Assistant Portal (ARAP)
ASP.NET Core (Backend) + Angular (Frontend)

A production-ready enterprise application for modernizing university research workflows.

📌 1. Project Overview

The Academic Research Assistant Portal (ARAP) streamlines and automates the entire academic research lifecycle for universities, advisors, and students.

The platform supports:

Proposal management

Document versioning

AI-powered text evaluation

Research progress tracking

Plagiarism detection

Project archiving

Advisor feedback workflows

The system starts as a Modular Monolith built with ASP.NET Core, and later extracts the Plagiarism Detection Module into a standalone Microservice.

🎯 2. Core Problem & Motivation

Universities still depend on:

Manual proposal submission

Slow email-based feedback

No unified progress tracking

External plagiarism checkers

Poor document organization

ARAP solves these through digital automation, centralized workflows, and integrated AI assistance.

👥 3. System Users

Students – submit research documents, receive AI guidance, track progress

Advisors – review work, annotate comments, approve stages, view plagiarism reports

Admins – manage research cycles, assign advisors, oversee users

🚀 4. Key Features
✅ Core Platform Features

📄 Proposal Submission & Review Flow

🤖 AI Document Structure Evaluation

🗂️ Document Versioning & Secure Storage

📊 Research Progress Tracking

🧠 AI RAG Knowledge Assistant

🔍 Plagiarism Detection Microservice

💬 Advisor–Student Feedback Threads

📚 Final Research Archive + Search

🧠 5. AI Integration
🎓 5.1 AI Features for Users

RAG-based source recommendations

Writing clarity improvement

Citation suggestions

Methodology refinement

Advisor support via evaluation summaries

🛠️ 5.2 AI in Development

Boilerplate code generation

Test generation

Documentation automation

CI/CD YAML generation

Architecture refactoring suggestions

🏗️ 6. System Architecture
🧩 6.1 Modular Monolith (Initial Phase)

Modules:

User & Role Management

Research Workflow

Document Storage

AI Assistant

Notifications

Observability & Logging

Everything lives in one deployable application.

🧬 6.2 Microservice Extraction Plan

Plagiarism & Document Analysis Module → Microservice

✨ Reasons for Extraction

CPU-intensive

AI-driven

Naturally asynchronous

Clear bounded context

Independent scaling required

Tech Stack

ASP.NET Core Web API

RabbitMQ / Azure Service Bus

Ocelot API Gateway

Independent Database

Docker-based deployment

📈 6.3 Observability

OpenTelemetry for distributed tracing

Serilog for structured logs

Prometheus + Grafana dashboards

Trace correlation between monolith & microservice

🎨 7. Angular Frontend

Includes:

Student Dashboard (AI chat, submissions, progress)

Advisor Dashboard (reviews, plagiarism reports)

Admin Dashboard (cycles, assignments, stats)

Responsive UI with E2E tests (Cypress/Playwright)

🔄 8. CI/CD & Deployment

GitHub Actions

Automated Docker builds

Test automation on commits

Staging environment deployment

Lint & code-quality checks

🔐 9. Security & Compliance

JWT Authentication

RBAC (Role-based authorization)

Encrypted file storage

Input sanitization

Rate-limiting

AI Interaction logging

⚠️ 10. AI Risks, Costs & Guardrails
Risks

Hallucinated citations

Embedding model token cost

Sensitive document privacy

False plagiarism scores

Guardrails

Advisor must validate AI suggestions

Token quota per user

AI action logging

Context filtering

Document isolation & encryption

📘 11. Short Technical Report
Monolith → Microservice Transition
11.1 Introduction

ARAP begins as a Modular Monolith for fast iteration.
Over time, the Plagiarism Detection Module is extracted into a Microservice for scalability and independence.

11.2 Architecture Evolution
Phase 1 — Modular Monolith

Benefits:

Fast development

Unified database

Simplified debugging

Phase 2 — Extraction Candidate

Plagiarism module selected due to:

Heavy computation

AI semantic models

Asynchronous behavior

Clear domain boundaries

Need for independent scaling

Phase 3 — Microservice Extraction

The new microservice uses:

Ocelot API Gateway

RabbitMQ / Azure Bus

OpenTelemetry tracing

Dedicated database

11.3 Trade-offs
⭐ Advantages

Fault isolation

Independent scaling

Tech flexibility

Better AI performance

⚠️ Challenges

More complex deployment

Message retries

Network latency

Increased hosting costs

11.4 AI Support in the Transition
AI-Assisted Development

Module refactoring

Service boundary suggestions

API/messaging boilerplate

Test generation

CI/CD pipeline automation

AI Inside the Microservice

Embeddings

Semantic similarity checks

Paraphrase detection

Overlap highlighting

This justifies making it a standalone, scalable service.