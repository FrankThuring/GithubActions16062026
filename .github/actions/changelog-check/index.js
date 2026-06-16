const { execSync } = require('node:child_process');

const baseRef = process.env.GITHUB_BASE_REF;
const changelogPath = process.env.INPUT_PATH || 'Github_Copilot_Actions-main/labs/CHANGELOG.md';

if (!baseRef) {
  console.log('Not a pull request event — skipping changelog check.');
  process.exit(0);
}

try {
  execSync(`git fetch --no-tags --depth=1 origin ${baseRef}`, { stdio: 'inherit' });
} catch {
  // fetch-depth: 0 on checkout already brings the branch; ignore failure here
}

const base = `origin/${baseRef}`;
const diff = execSync(`git diff --name-only ${base}...HEAD`, { encoding: 'utf8' });
const files = diff.split('\n').map(s => s.trim()).filter(Boolean);

console.log('Changed files in PR:');
for (const f of files) console.log(`  ${f}`);

if (files.includes(changelogPath)) {
  console.log(`\n::notice::${changelogPath} was updated.`);
  process.exit(0);
}

console.log(`\n::error::${changelogPath} was not updated in this PR. Add an entry under [Unreleased].`);
process.exit(1);
