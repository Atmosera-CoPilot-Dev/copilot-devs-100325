# Security Policy

Purpose  
Baseline security expectations for the ASP.NET Core Web API + React + MongoDB invoicing platform deployed on Azure. This document complements detailed policies/standards and is version-controlled with the codebase.

## 1. Objectives
- Protect customer, financial, and invoice data (confidentiality, integrity, availability).
- Enforce least privilege & Zero Trust.
- Ensure traceability (audit-ready logging).
- Rapid detection & response to security events.
- Embed security into the SDLC (shift-left + continuous verification).

## 2. Scope
- Backend: ASP.NET Core REST API
- Frontend: React SPA
- Data: MongoDB (Atlas or Azure Cosmos DB for Mongo API), cache (if added)
- Infrastructure: Azure (App Service / Container Apps, Azure Key Vault, Azure Monitor, Front Door / Application Gateway, Storage)
- CI/CD: Git repository + pipeline (GitHub Actions / Azure DevOps)
- Third-party integrations (email, payments, identity provider)

## 3. Roles (RACI High-Level)
- Product Owner: Data use approval
- Tech Lead: Architecture sign-off, threat modeling facilitator
- Dev Team: Secure coding, dependency hygiene
- DevOps/Platform: Infra as Code, hardening, secrets lifecycle
- Security Champion: Pull request security checklist
- Security/Ops (if available): Monitoring, incident coordination

## 4. Data Classification
- Restricted: Customer PII, authentication tokens
- Confidential: Invoice details, financial aggregates, internal metrics
- Internal: Non-sensitive backlog, configuration templates
- Public: Marketing assets
Handling Rules:
- Restricted/Confidential encrypted in transit (TLS 1.2+) and at rest (MongoDB encrypted + disk encryption).
- No production data in lower environments (use synthetic).
- Data exports logged and access-controlled.

## 5. Authentication & Authorization
- Auth: OpenID Connect / OAuth2 (e.g., Azure AD / Entra ID / Auth0)
- Tokens: Short-lived access + refresh rotation; store access token in memory (SPA) not localStorage.
- API: Enforce audience, issuer validation; require HTTPS.
- Authorization: Role + (optional) claim-based / resource-scoped (e.g., invoice ownership).
- Admin endpoints segregated with separate role & stricter conditional access (MFA mandatory).

## 6. Session & Token Security
- No long-lived static API keys.
- Revoke on password reset / suspected compromise.
- Use sliding refresh tokens + reuse detection (if provider supports).

## 7. Secrets & Keys
- Managed in Azure Key Vault (never in source or pipeline variables in plain text).
- Rotate: App secrets ≤ 180 days, DB credentials ≤ 90 days (if not using managed identity).
- Use Managed Identity where possible (App Service → Key Vault).
- Certificate lifecycle tracked (auto-renew via Key Vault + DNS / ACME where applicable).

## 8. Secure SDLC
- Threat Modeling: At feature epics (STRIDE-lite).
- Pull Request Template: Security checklist (input validation, authZ impact, secrets).
- Automation Gates:
  - SAST: Each PR (fail on High severity).
  - Dependency Scanning (SCA): Fail on Known Critical (no override without approval).
  - Secrets Scan: Block commit (pre-commit + CI).
  - Container/Image Scan (if containers used) before publish.
- Security Unit/Integration Tests: Authorization boundaries, negative cases.

## 9. Dependency & Supply Chain
- Pin versions; no wildcards for critical libs.
- Maintain SBOM (e.g., Syft or built-in SCA tool).
- Remove unused packages monthly.
- Use repository-level allowlist for runtime-critical packages.

## 10. Input & Data Validation
- Enforce server-side validation (FluentValidation / DataAnnotations).
- Reject overlong payloads (size limits).
- Sanitize for logs (no raw user input).
- Use strict JSON schema for external-facing APIs.

## 11. Logging & Monitoring
- Centralize logs (Azure Monitor / Log Analytics).
- Log categories: Auth events, access denials, privilege changes, invoice mutations, report exports, error stacks (sanitized).
- Correlation ID per request (trace through frontend → backend → DB).
- Sensitive data redaction: Never log secrets, tokens, PII fields (mask at source).
- Alerting: Auth failure spikes, anomalous invoice generation volume, repeated 5xx, unusual report export frequency.

## 12. Error Handling
- Generic messages to clients (no stack traces).
- Detailed exception only in secure log sink.
- Distinguish 4xx (client) vs 5xx (server) for monitoring thresholds.

## 13. Vulnerability Management
- Scan frequency: SAST/SCA per PR; container images per build; infra (IaC) policy scan per change.
- Patch SLA: Critical 7 days, High 14 days, Medium 30 days.
- Track exceptions with expiry & compensating controls.

## 14. Configuration & Hardening
- Infrastructure as Code (Bicep/Terraform) version-controlled.
- Principle of least privilege for service principals / managed identities.
- Enforce TLS only; disable weak ciphers.
- HTTP security headers: CSP (restrict origins), HSTS, X-Content-Type-Options, X-Frame-Options (DENY), Referrer-Policy.
- CSP tuned to block inline scripts (use hashes/nonces if needed).

## 15. Data Protection & Encryption
- At Rest: Provider-managed encryption + customer-managed key (if required).
- In Transit: TLS 1.2+; prefer TLS 1.3 when available.
- Field-level encryption (if storing sensitive identity attributes).
- Backups encrypted; access logged; periodic restore test.

## 16. Reporting & Analytics Security
- Limit report queries with server-side pagination & authorization checks.
- Aggregated financial views only for authorized roles.
- Prevent enumeration: Filter only by authorized customer context.

## 17. Infrastructure (Azure) Security
- Network: Private endpoints for DB; disable public where feasible.
- WAF (Azure Front Door or Application Gateway) in front of API.
- DDoS Protection Standard if public exposure.
- Defender for Cloud recommendations monitored.
- Use Azure Policy to block non-compliant resource deployments.
- Enable diagnostic logs for App Service / Key Vault / Mongo (audit trail).

## 18. Third-Party & Integrations
- Validate outbound calls (allowlist domains).
- Minimal scopes for API tokens (principle of least privilege).
- Monitor usage anomalies (rate vs baseline).

## 19. Build & Release Security
- Signed artifacts (if container: signed image).
- Immutable build pipeline (no ad-hoc changes in release stage).
- Environment-based config only (no environment logic in code branches).
- Deployment approvals required for production.

## 20. Incident Response (Lightweight)
- Detect → Triage (Severity) → Contain → Eradicate → Recover → Postmortem.
- Escalation triggers: Data exfil suspected, auth bypass, persistent 5xx anomaly, invoice tampering.
- Maintain runbook references (external doc).

## 21. Metrics (Sample)
- MTTR security incidents
- % High/Critical vulns within SLA
- Dependency freshness (avg days behind latest patch)
- Failed vs successful auth ratio trend
- Secrets rotation compliance %

## 22. Privacy & Minimal Data
- Collect only required invoice/customer fields.
- Avoid storing full payment instrument (use tokenized services).
- Data subject deletion workflow (if regulatory obligations apply).

## 23. Frontend Security (React)
- No dangerous HTML injection; use framework sanitization.
- Avoid storing tokens in localStorage; use memory + silent renew.
- Implement anti-CSRF (not needed if SPA uses only bearer tokens with no cookies; if cookies used → same-site=strict + anti-CSRF token).
- Integrity of third-party scripts (subresource integrity).

## 24. Policy Enforcement & Reviews
- Quarterly security review of this file.
- Automated checks (CI) must pass before merge.
- Any exception documented in /security-exceptions.md with expiry.

## 25. Exceptions
- Must include: rationale, risk, compensating control, owner, expiration date.
- Auto-review at or before expiry.

## 26. Quick Developer Checklist (Pre-Merge)
- [ ] Threat model updated (if new trust boundary)
- [ ] Input validation & authZ tests added
- [ ] No secrets committed
- [ ] Dependencies scanned & no unresolved Critical/High
- [ ] Logs redact sensitive data
- [ ] Feature behind proper roles/claims
- [ ] API changes versioned & documented
- [ ] Frontend avoids insecure storage of tokens

## 27. Reference Standards (External)
- OWASP ASVS (API sections)
- OWASP Top 10
- CWE Top 25 (awareness)
- OAuth 2.1 / OIDC Best Current Practices

## 28. Version & Ownership
- Owner: Security Champion / Tech Lead
- Review Cycle: Quarterly or on major architectural change
- Current Version: 1.0.0

---
Keeping this file concise ensures adoption. For deeper control detail, link to standards (not duplicated here).
