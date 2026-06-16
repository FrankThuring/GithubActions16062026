import { describe, it, expect } from 'vitest';
import {
  daysBetween,
  overdueInvoices,
  totalOutstandingCents,
  type Invoice,
} from './invoices.js';

/**
 * Seed tests so the lab repo is green from the start.
 * In Lab 5 the participants generate MORE tests with Copilot (e.g. for
 * formatEuros and additional edge cases) and review the assertions.
 */

const invoices: Invoice[] = [
  { id: 'A', amountCents: 10_00, issuedOn: '2026-01-01', paid: false },
  { id: 'B', amountCents: 25_50, issuedOn: '2026-03-01', paid: true },
  { id: 'C', amountCents: 5_00, issuedOn: '2026-05-01', paid: false },
];

describe('daysBetween', () => {
  it('counts whole days forward', () => {
    expect(daysBetween('2026-01-01', '2026-01-11')).toBe(10);
  });

  it('throws on an invalid date', () => {
    expect(() => daysBetween('not-a-date', '2026-01-01')).toThrow();
  });
});

describe('overdueInvoices', () => {
  it('returns only unpaid invoices older than the threshold, oldest first', () => {
    const result = overdueInvoices(invoices, 30, '2026-06-01');
    expect(result.map((i) => i.id)).toEqual(['A', 'C']);
  });

  it('excludes paid invoices even if old', () => {
    const result = overdueInvoices(invoices, 0, '2026-06-01');
    expect(result.every((i) => !i.paid)).toBe(true);
  });
});

describe('totalOutstandingCents', () => {
  it('sums only unpaid invoices', () => {
    expect(totalOutstandingCents(invoices)).toBe(15_00);
  });
});

/** Returns the single most overdue unpaid invoice, or null. */
export function mostOverdue(invoices: Invoice[], asOf: string): Invoice | null {
  const overdue = overdueInvoices(invoices, 0, asOf);
  return overdue.length > 0 ? overdue[0] : null;
}