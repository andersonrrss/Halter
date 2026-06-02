import { ExerciseType } from "./enums/exercise-type.enum";

export interface WorkoutResponse {
  /** @format uuid */
  id?: string;
  name?: string | null;
}

export interface WorkoutRequest {
  /** @minLength 1 */
  name: string;
  /** @format uuid */
  routineId?: string;
}

export interface WorkoutWithExercisesResponse {
  /** @format uuid */
  id?: string;
  name?: string | null;
  workoutExercises?: WorkoutExerciseResponse[] | null;
}

export interface WorkoutExerciseRequest {
  /** @format int32 */
  exerciseId?: number;
  /** @format int32 */
  sets?: number;
  exerciseType?: ExerciseType;
  values?: number[] | null;
  /** @format int32 */
  isometricHoldSeconds?: number | null;
}

export interface WorkoutExerciseResponse {
  /** @format uuid */
  id?: string;
  /** @format int32 */
  exerciseId?: number;
  exerciseName?: string | null;
  exerciseType?: ExerciseType;
  values?: number[] | null;
  /** @format int32 */
  sets?: number;
  /** @format int32 */
  isometricHoldSeconds?: number | null;
}