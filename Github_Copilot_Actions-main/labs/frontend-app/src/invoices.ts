/**
 * Invoice domain helpers.
 *
 * This module is intentionally small and well-typed so it is a good playground
 * for GitHub Copilot demos: explaining code, generating tests, and refactoring.
 */

export interface Invoice {
  id: string;
  /** Amount due in euro cents (integer) to avoid floating-point money bugs. */
  amountCents: number;
  /** ISO date the invoice was issued, e.g. "2026-05-01". */
  issuedOn: string;
  /** Whether the invoice has been paid. */
  paid: boolean;
}

/** Number of whole days between two ISO dates (b - a). */
export function daysBetween(a: string, b: string): number {
  const msPerDay = 24 * 60 * 60 * 1000;
  const start = Date.parse(a);
  const end = Date.parse(b);
  if (Number.isNaN(start) || Number.isNaN(end)) {
    throw new Error(`Invalid date passed to daysBetween: "${a}" / "${b}"`);
  }
  return Math.floor((end - start) / msPerDay);
}

/**
 * Returns the unpaid invoices that are overdue by strictly more than `days`,
 * as measured against `asOf`, sorted oldest-issued first.
 */
export function overdueInvoices(
  invoices: Invoice[],
  days: number,
  asOf: string,
): Invoice[] {
  return invoices
    .filter((inv) => !inv.paid && daysBetween(inv.issuedOn, asOf) > days)
    .sort((a, b) => Date.parse(a.issuedOn) - Date.parse(b.issuedOn));
}

/** Total outstanding amount (in cents) across all unpaid invoices. */
export function totalOutstandingCents(invoices: Invoice[]): number {
  return invoices
    .filter((inv) => !inv.paid)
    .reduce((sum, inv) => sum + inv.amountCents, 0);
}

/** Formats euro cents as a localized string, e.g. 12345 -> "€123,45". */
export function formatEuros(amountCents: number): string {
  return new Intl.NumberFormat('nl-NL', {
    style: 'currency',
    currency: 'EUR',
  }).format(amountCents / 100);
}
