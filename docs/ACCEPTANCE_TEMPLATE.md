# First-playable acceptance specification template

Status: UNFILLED. This is a format, not approved game design.

## Environment
Editor version, target platform/input, commit and build identifier.

## Rule MECH-001
Source decision:
Player action:
Preconditions:
State transition:
Visible feedback:
Invalid action:
Reset and persistence behavior:

## Golden playthrough
Starting state:
Exact input/action sequence:
Expected intermediate states:
Win condition:
Failure condition and retry:

## Verification record
| Check | Expected result | Actual result | Evidence |
| --- | --- | --- | --- |
| Unity import/compile | No errors | Not run | |
| Rule tests | Approved scenarios pass | Not run | |
| Target build launch | Opens successfully | Not run | |
| Success path | Approved win state | Not run | |
| Failure/recovery | Approved reset state | Not run | |
| Replay | Consistent initial state | Not run | |
| Human playtest | Brian accepts the slice | Not run | |

Remove irrelevant checks only after the design makes their applicability clear. Record defects as linked issues; do not mark acceptance from source inspection alone.
