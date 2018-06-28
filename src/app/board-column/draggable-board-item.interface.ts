interface DraggableBoardItem extends BoardItem {
  effectAllowed: string;
}

interface BoardItem {
  title: string;
  content: string;
}
