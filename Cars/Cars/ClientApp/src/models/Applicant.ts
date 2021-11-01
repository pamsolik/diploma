import {City} from "./City";

export interface Applicant {
  id: number,
  email: string,
  phoneNumber: string,
  name: string,
  surname: string,
  description: string,
  profilePicture: string,
  city: City,
  github: string,
  linkedIn: string,
  skills: any,
  education: any,
  experience: any,
}
