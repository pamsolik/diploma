
export interface RecruitmentsFilters {
  pageSize: number,
  pageIndex: number,
  searchString: string,
  technology: number,
  sortOrder: number,
}

export function FiltersDefault(): RecruitmentsFilters {
  return {
    pageSize: 25,
    pageIndex: 0,
    searchString: "",
    technology: 0,
    sortOrder: 0,
  }
}
