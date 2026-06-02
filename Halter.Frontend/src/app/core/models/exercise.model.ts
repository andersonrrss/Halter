import { TimeConstraint } from "./enums/time-constraint.model";

export interface ExerciseResponse {
  /** @format int32 */
  id?: number;
  name?: string | null;
  timeConstraint?: TimeConstraint;
  muscleGroup?: MuscleGroup;
}

export interface ExerciseSimpleResponse {
  /** @format int32 */
  id?: number;
  name?: string | null;
}





export interface MuscleGroup {
  /** @format int32 */
  id?: number;
  name?: string | null;
}