import { WorkoutResponse } from "./workout.model";

export interface RoutineRequest {
  /** @minLength 1 */
  name: string;
}

export interface RoutineResponse {
  /** @format uuid */
  id?: string;
  name?: string | null;
  workouts?: WorkoutResponse[] | null;
}