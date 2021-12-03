import {Applicant} from "./Applicant";

export interface UserList {
  items: UserAdminView[],
  totalItems: number,
  pageIndex: number,
  pageSize: number
}

export interface UserAdminView extends Applicant {

}

export interface UserFilters {
  searchTerm: string,
  pageIndex: number,
  pageSize: number
}

export function UserFiltersDefault(): UserFilters {
  return {
    searchTerm: "",
    pageSize: 10,
    pageIndex: 0,
  }
}
