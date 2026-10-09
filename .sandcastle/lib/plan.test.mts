import assert from "node:assert/strict";
import { describe, it } from "node:test";
import { readyPicks } from "./plan.mts";

const ready = [{ number: 1 }, { number: 2 }, { number: 3 }];
const ids = (...values: string[]) => values.map((id) => ({ id }));

describe("readyPicks", () => {
  it("keeps the ready issues the plan names, in plan order", () => {
    assert.deepEqual(readyPicks(ids("3", "1"), ready, () => {}), [{ number: 3 }, { number: 1 }]);
  });

  it("skips ids that aren't ready issues", () => {
    const warnings: string[] = [];
    assert.deepEqual(readyPicks(ids("9", "2"), ready, (line) => warnings.push(line)), [{ number: 2 }]);
    assert.match(warnings[0]!, /Skipping 9/);
  });

  it("keeps each issue once when the plan repeats it", () => {
    const warnings: string[] = [];
    assert.deepEqual(readyPicks(ids("2", "2", "#2", " 2 "), ready, (line) => warnings.push(line)), [{ number: 2 }]);
    assert.equal(warnings.length, 3);
  });
});
