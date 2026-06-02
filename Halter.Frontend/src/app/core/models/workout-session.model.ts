import { ExerciseType } from "./enums/exercise-type.enum";
import { ExerciseSimpleResponse } from "./exercise.model";

export interface UnfinishedWorkoutSessionResponse {
  /** @format uuid */
  id?: string;
  /** @format uuid */
  workoutId?: string | null;
  /** @format date-time */
  startedAt?: string;
  /** @format date-time */
  pausedAt?: string | null;
  /** @format int32 */
  totalPausedSeconds?: number;
  exerciseEntries?: ExerciseEntryResponse[] | null;
}

export interface FinishedWorkoutSessionResponse {
  /** @format uuid */
  id?: string;
  /** @format uuid */
  workoutId?: string | null;
  /** @format date-time */
  startedAt?: string;
  /** @format date-time */
  finishedAt?: string;
  /** @format int32 */
  totalPausedSeconds?: number;
  /** @format date-span */
  totalTime?: string;
  exerciseEntries?: ExerciseEntryResponse[] | null;
}

export interface ExerciseEntryRequest {
  /** @format uuid */
  workoutExerciseId?: string;
  /** @format int32 */
  valueAchieved?: number;
  /** @format int32 */
  setsCompleted?: number;
  /** @format double */
  maxWeight?: number | null;
}

export interface ExerciseEntryResponse {
  /** @format uuid */
  id?: string;
  exercise?: ExerciseSimpleResponse;
  /** @format uuid */
  workoutSessionId?: string;
  exerciseType?: ExerciseType;
  /** @format date-time */
  completedAt?: string;
  /** @format int32 */
  valueAchieved?: number;
  /** @format int32 */
  setsCompleted?: number;
  /** @format double */
  maxWeight?: number | null;
}